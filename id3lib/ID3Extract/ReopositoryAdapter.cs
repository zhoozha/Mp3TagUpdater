using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TagExtract.Repository;
using System.Data.SqlServerCe;
using Id3Lib;
using System.IO;
using System.Diagnostics;

namespace TagExtract
{
    public class ReopositoryAdapter : IDisposable
    {
        SqlCeConnection _connection;

        public ReopositoryAdapter(SqlCeConnection connection)
        {
            _connection = connection;
        }

        public void PublishFrames(string fileName)
        {
            using (RepositoryDataContext dataContext = new RepositoryDataContext(_connection))
            {
                using (Stream stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
                {
                    try
                    {
                        FrameModel tagModel = FrameManager.Deserialize(stream);
                        SourceFile sourceFile = new SourceFile()
                        {
                            FileName = Path.GetFileName(fileName),
                            FilePath = Path.GetDirectoryName(fileName)
                        };

                        foreach (IFrame frame in tagModel)
                        {
                            SourceFileTag fileTag = new SourceFileTag() { Tag = frame.FrameId };
                            sourceFile.SourceFileTag.Add(fileTag);

                            if (frame is FrameText)
                            {
                                FrameText frameType = (FrameText)frame;
                                fileTag.TagText = new TagText()
                                {
                                    Text = frameType.Text,
                                    TextEncodingId = (byte)frameType.TextCode
                                };
                                continue;
                            }

                            if (frame is FrameTextUserDef)
                            {
                                FrameTextUserDef frameType = (FrameTextUserDef)frame;
                                fileTag.TagText = new TagText()
                                {
                                    Text = frameType.Text,
                                    Description = frameType.Description,
                                    TextEncodingId = (byte)frameType.TextCode
                                };
                                continue;
                            }

                            if (frame is FrameUrl)
                            {
                                FrameUrl frameType = (FrameUrl)frame;
                                fileTag.TagText = new TagText()
                                {
                                    Text = frameType.Url
                                };
                                continue;
                            }

                            if (frame is FrameUrlUserDef)
                            {
                                FrameUrlUserDef frameType = (FrameUrlUserDef)frame;
                                fileTag.TagText = new TagText()
                                {
                                    Text = frameType.URL,
                                    Description = frameType.Description,
                                    TextEncodingId = (byte)frameType.TextCode
                                };
                                continue;
                            }

                            if (frame is FrameFullText)
                            {
                                FrameFullText frameType = (FrameFullText)frame;
                                fileTag.TagFullText = new TagFullText()
                                {
                                    TextEncodingId = (byte)frameType.TextCode,
                                    Description = frameType.Description,
                                    TextLanguage = frameType.Language,
                                    Comment = frameType.Text
                                };
                                continue;
                            }

                            if (frame is FramePicture)
                            {
                                FramePicture frameType = (FramePicture)frame;
                                fileTag.TagPicture = new TagPicture()
                                {
                                    PictureTypeId = (byte)frameType.PictureType,
                                    MimeType = frameType.Mime,
                                    Description = frameType.Description,
                                    BinaryImage = frameType.PictureData
                                };
                                continue;
                            }

                            if (frame is FrameBinary)
                            {
                                FrameBinary frameType = (FrameBinary)frame;
                                fileTag.TagBinary = new TagBinary()
                                {
                                    TextEncodingId = (byte)frameType.TextEncoding,
                                    MimeType = frameType.Mime,
                                    Description = frameType.Description,
                                    BinaryObject = frameType.ObjectData
                                };
                                continue;
                            }
                        }
                        dataContext.SourceFile.InsertOnSubmit(sourceFile);
                        dataContext.SubmitChanges();
                    }
                    catch (NotImplementedException e)
                    {
                        Console.WriteLine("{0}:{1}", fileName, e.Message);
                    }
                }
            }
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
