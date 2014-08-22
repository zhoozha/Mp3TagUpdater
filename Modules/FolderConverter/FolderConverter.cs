using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Unity;
using System.ComponentModel.Composition;
using System.Threading;
using Mp3Lib;
using Id3Lib;
using Mp3.Infrastructure;
using Mp3.Infrastructure.Models;

namespace Modules.FolderConverter
{
    public class FolderConverter : IFolderConverter
    {
        Dictionary<string, string[]> folders = new Dictionary<string, string[]>();
        string _rootFolder;
        int _total;
        int _current;

        ILoggerFacade _logger;
        IProgressFacade _progress;
        IUnityContainer _container;

        private struct FileContainer
        {
            public string source;
            public string target;
        }

        public bool UpdateMp3Tag { get; set; }

        public FolderConverter(ILoggerFacade logger, IUnityContainer container, IProgressFacade progress)
        {
            _logger = logger;
            _container = container;
            _progress = progress;
        }

        private void Log(string msg)
        {
            if (_logger == null)
            {
                return;
            }
            _logger.Log(msg, Category.Info, Priority.None);
        }

        void ProgressEvent(double current, double total)
        {
            if (_progress != null)
            {
                _progress.Update(current, _total);
                if (current >= total && total < 100) //sleep for 1 sec if # of files < 100
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private Dictionary<string, string[]> ParseFolder(string sourceRoot)
        {
            string[] files = Directory.GetFiles(sourceRoot);
            folders.Add(sourceRoot.Substring(_rootFolder.Length, sourceRoot.Length - _rootFolder.Length), Directory.GetFiles(sourceRoot));
            Log(sourceRoot + " folder is added with " + files.Length.ToString() + " files");
            _total += files.Length;
            foreach (string folder in Directory.GetDirectories(sourceRoot))
            {

                ParseFolder(folder);
            }

            ProgressEvent(0, _total);
            return folders;
        }

        private Dictionary<string, FileContainer[]> GetTargetFolder(string targetRoot, Dictionary<string, string[]> sourceFolders)
        {
            Dictionary<string, FileContainer[]> toSave = new Dictionary<string, FileContainer[]>();
            //IStringConverter converter = _container.Resolve<IStringConverter>();
            List<FileContainer> files = new List<FileContainer>();
            IStringConverter _converter = _container.Resolve<IStringConverter>();
            foreach (string key in sourceFolders.Keys)
            {
                //make target folder name    
                files.Clear();
                foreach (string file in sourceFolders[key])
                {
                    files.Add(new FileContainer()
                    {
                        source = file,
                        target = _converter.Convert(Path.GetFileName(file))
                    });
                }
                toSave.Add(_converter.Convert(key), files.ToArray());
                Log(_converter.Convert(key) + " folder added with " + files.Count.ToString() + " files");
            }

            return toSave;
        }

        private void SaveStructure(Dictionary<string, FileContainer[]> toSave, string targetRoot)
        {
            string path;
            if (!Directory.Exists(targetRoot))
                Directory.CreateDirectory(targetRoot);

            IStringConverter converter = _container.Resolve<IStringConverter>();
            targetRoot = (targetRoot[targetRoot.Length - 1] == '\\' ? targetRoot : targetRoot + "\\");

            foreach (string folder in toSave.Keys)
            {
                if (folder.Length == 0)
                    path = targetRoot;
                else
                    path = targetRoot + (folder[0] == '\\' ? folder.Substring(1, folder.Length - 1) : folder) + '\\';

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                foreach (var file in toSave[folder])
                {
                    if (File.Exists(path + file.target))
                        File.Delete(path + file.target);
                    File.Copy(file.source, path + file.target, true);
                    File.SetAttributes(path + file.target, FileAttributes.Normal);
                    if (UpdateMp3Tag)
                        ProcessFile(path + file.target, converter);
                    Log(file.source + " is copied to " + file.target);
                    _current++;
                    ProgressEvent(_current, _total);
                }
            }
            ProgressEvent(0, _total);
        }

        string ASCIIToUnicode(string source, IConverter<string> converter)
        {
            Encoding utf = Encoding.GetEncoding("windows-1251");
            Encoding ascii = Encoding.ASCII;
            char[] chars = source.ToCharArray();
            List<byte> bytes = new List<byte>();

            for (int i = 0; i < chars.Length; i++)
            {
                bytes.Add((byte)System.Convert.ToInt32(chars[i]));
            }
            return ascii.GetString(utf.GetBytes(converter.Convert(utf.GetString(bytes.ToArray()))));
        }

        private void ProcessFile(string filename, IConverter<string> converter)
        {
            Mp3File mp3File = null;

            try
            {
                // create mp3 file wrapper; open it and read the tags
                mp3File = new Mp3File(filename);
            }
            catch (Exception ex)
            {
                Log("Error Reading Tag for '" + filename + "'" + ex.Message);
                return;
            }

            if (mp3File.TagModel != null)
            {
                TagHandler tag = new TagHandler(mp3File.TagModel);
                tag.Album = converter.Convert(tag.Album);
                tag.Artist = converter.Convert(tag.Artist);
                tag.Composer = converter.Convert(tag.Composer);
                tag.Song = converter.Convert(tag.Song);
                tag.Title = converter.Convert(tag.Title);
                mp3File.Update();
            }
        }

        public void Convert(string sourceFolder, string targetFolder, bool updateMp3Tag)
        {
            this.UpdateMp3Tag = updateMp3Tag;
            this.Convert(sourceFolder, targetFolder);
        }

        public void Convert(string sourceRoot, string targetRoot)
        {
            _total = 0;
            _current = 0;
            _rootFolder = sourceRoot;
            folders.Clear();
            SaveStructure(
            GetTargetFolder(targetRoot,
            ParseFolder(sourceRoot)), targetRoot);
        }

        public string Convert(string obj)
        {
            Convert(obj, obj);
            return null;
        }


        public void Convert(Mp3Folders model)
        {
            this.Convert(model.FolderFrom, model.FolderTo, model.ProcessTags);
        }
    }
}
