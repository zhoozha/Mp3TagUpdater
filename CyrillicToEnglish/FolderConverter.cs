using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Mp3Lib;
using Id3Lib;

namespace MP3TagUpdater
{
    public delegate void LogMsgHandler(string msg);
    public delegate void ProgressHandler(double current, double total);

    public class FolderConverter
    {
        public event LogMsgHandler LogEvent;
        public event ProgressHandler ProgressEvent;
        Dictionary<string, string[]> folders = new Dictionary<string, string[]>();
        string _rootFolder;
        bool _log;
        int _total;
        int _current;
        bool _updateTag;

        private struct FileContainer
        {
            public string source;
            public string target;
        }

        public FolderConverter(bool showLog, bool updateMP3Tag)
        {
            _log = showLog;
            _updateTag = updateMP3Tag;
        }

        private void Log(string msg)
        {
            if (!_log)
                return;
            LogEvent(msg);
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
            CharConverter converter = new CharConverter();
            List<FileContainer> files = new List<FileContainer>();

            foreach (string key in sourceFolders.Keys)
            {
                //make target folder name    
                files.Clear();
                foreach (string file in sourceFolders[key])
                {
                    files.Add(new FileContainer()
                    {
                        source = file,
                        target = converter.ConvertString(Path.GetFileName(file))
                    });
                }
                toSave.Add(converter.ConvertString(key), files.ToArray());
                Log(converter.ConvertString(key) + " folder added with " + files.Count.ToString() + " files");
            }

            return toSave;
        }

        private void SaveStructure(Dictionary<string, FileContainer[]> toSave, string targetRoot)
        {
            string path;
            if (!Directory.Exists(targetRoot))
                Directory.CreateDirectory(targetRoot);
            CharConverter converter = new CharConverter();
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
                    if (_updateTag)
                        ProcessFile(path + file.target, converter);
                    Log(file.source + " is copied to " + file.target);
                    _current++;
                    ProgressEvent(_current, _total);
                }
            }
        }

        string ASCIIToUnicode(string source, CharConverter converter)
        {
            Encoding utf = Encoding.GetEncoding("windows-1251");
            Encoding ascii = Encoding.ASCII;
            char[] chars = source.ToCharArray();
            List<byte> bytes = new List<byte>();
            int ic;
            for (int i = 0; i < chars.Length; i++)
            {
                bytes.Add((byte)System.Convert.ToInt32(chars[i]));
            }
            return ascii.GetString(utf.GetBytes(converter.ConvertString(utf.GetString(bytes.ToArray()))));
        }

        private void ProcessFile(string filename, CharConverter converter)
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
                tag.Album = converter.ConvertString(tag.Album);
                tag.Artist = converter.ConvertString(tag.Artist);
                tag.Composer = converter.ConvertString(tag.Composer);
                tag.Song = converter.ConvertString(tag.Song);
                tag.Title = converter.ConvertString(tag.Title);
                mp3File.Update();
            }
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
    }
}
