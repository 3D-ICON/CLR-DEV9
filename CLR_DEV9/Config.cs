﻿using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace CLRDEV9
{
    [DataContract]
    class Config
    {
        [DataMember]
        public string Eth;
        [DataMember]
        public string Hdd;
        [DataMember]
        public int HddSize;
        [DataMember]
        public bool HddEnable;
        [DataMember]
        public bool EthEnable;

        public static void DoConfig(string iniFolderPath, string iniFileName)
        {
            ConfigForm cfgF = new ConfigForm();
            cfgF.iniFolder = iniFolderPath;
            cfgF.ShowDialog();
            cfgF.Dispose();
        }

        public static void SaveConf(string iniFolderPath, string iniFileName)
        {
            string filePath = iniFolderPath + "\\" + iniFileName;
            DataContractSerializer ConfSerializer = new DataContractSerializer(typeof(Config));

            var settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t"
            };

            FileStream Writer = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
            using (var writer = XmlWriter.Create(Writer, settings))
            {
                ConfSerializer.WriteObject(writer, DEV9Header.config);
            }
            Writer.Close();
        }

        public static void LoadConf(string iniFolderPath, string iniFileName)
        {
            string filePath = iniFolderPath + "\\" + iniFileName;

            if (File.Exists(filePath))
            {
                DataContractSerializer ConfSerializer = new DataContractSerializer(typeof(Config));
                FileStream Reader = new FileStream(filePath, FileMode.Open);

                DEV9Header.config = (Config)ConfSerializer.ReadObject(Reader);

                Reader.Close();
                return;
            }

            DEV9Header.config = new Config();
            DEV9Header.config.Hdd = DEV9Header.HDD_DEF;
            DEV9Header.config.HddSize = 8 * 1024;
            DEV9Header.config.Eth = DEV9Header.ETH_DEF;
            DEV9Header.config.EthEnable = true;
            DEV9Header.config.HddEnable = false;

            SaveConf(iniFolderPath, iniFileName);
        }
    }
}
