using System;
using System.Xml;
using System.IO;
using TaleWorlds.Library;
using System.Windows.Forms;
using System.Xml.Linq;

namespace FightingTogetherRelationship
{
    public class FTRConfig
    {
        private static FTRConfig configInstance = null;
        public FTRInitialize FTRInitialize { get; set; } = new FTRInitialize();
        public FTRCore FTRCore { get; set; } = new FTRCore();

        public XDocument lang = null;

        //Creates a new instance of FTRConfig
        public static FTRConfig newInstance
        {
            get
            {
                if (FTRConfig.configInstance == null)
                {
                    FTRConfig.configInstance = new FTRConfig();
                }
                return FTRConfig.configInstance;
            }
        }
        //Loads the values of the xml file
        public void LoadConfig(String configPath)
        {
            if (!File.Exists(configPath))
            {
                throw new Exception("The config from: " + configPath + " could not be loaded.");
            }
            XmlDocument cfg = new XmlDocument();
            cfg.Load(configPath);
            foreach(XmlNode node in cfg.DocumentElement)
            {
                switch (node.Name)
                {
                    case "Custom_Values":
                        if(node.Attributes[0].InnerText.Equals("true"))
                        {
                            FTRInitialize.EnableCustomValues = true;
                            foreach (XmlNode childNode in node.ChildNodes)
                            {
                                switch (childNode.Name)
                                {
                                    case "Minimum_Enemies":
                                        int minimumEnemy = int.Parse(childNode.InnerText);
                                        FTRInitialize.MinimumEnemy = minimumEnemy;
                                        break;
                                    case "Minimum_Allies":
                                        int minimumAlly = int.Parse(childNode.InnerText);
                                        FTRInitialize.MinimumAlly = minimumAlly;
                                        break;
                                    case "Relation_Cap":
                                        int relationCap = int.Parse(childNode.InnerText);
                                        FTRInitialize.RelationCap = relationCap;
                                        break;
                                }
                            }
                        }
                        else
                        {
                            FTRInitialize.EnableCustomValues = false;
                        }
                        break;
                    case "Relationship_Loss":
                        if (node.Attributes[0].InnerText.Equals("true"))
                        {
                            FTRInitialize.EnableRelationshipLoss = true;
                            foreach (XmlNode childNode in node.ChildNodes)
                            {
                                switch (childNode.Name)
                                {
                                    case "Loss_Multiplier":
                                        double LossMultiplier = double.Parse(childNode.InnerText);
                                        FTRInitialize.LossMultiplier = LossMultiplier;
                                        break;
                                }
                            }
                        }
                        else
                        {
                            FTRInitialize.EnableRelationshipLoss = false;
                        }
                        break;
                    case "Relationship_Gain":
                        if (node.Attributes[0].InnerText.Equals("true"))
                        {
                            FTRInitialize.EnableRelationshipGain = true;
                            foreach (XmlNode childNode in node.ChildNodes)
                            {
                                switch (childNode.Name)
                                {
                                    case "Gain_Multiplier":
                                        double GainMultiplier = double.Parse(childNode.InnerText);
                                        FTRInitialize.GainMultiplier = GainMultiplier;
                                        break;
                                }
                            }
                        }
                        else
                        {
                            FTRInitialize.EnableRelationshipGain = false;
                        }
                        break;
                    case "Companions":
                        if (node.Attributes[0].InnerText.Equals("true"))
                        {
                            FTRInitialize.EnableCompanions = true;
                            foreach (XmlNode childNode in node.ChildNodes)
                            {
                                switch (childNode.Name)
                                {
                                    case "Base_Value":
                                        int companionBaseValue = int.Parse(childNode.InnerText);
                                        FTRInitialize.CompanionBaseValue = companionBaseValue;
                                        break;
                                }
                            }
                        }
                        else
                        {
                            FTRInitialize.EnableCompanions = false;
                        }
                        break;
                    case "Spouse":
                        if (node.Attributes[0].InnerText.Equals("true"))
                        {
                            FTRInitialize.EnableSpouse = true;
                            foreach (XmlNode childNode in node.ChildNodes)
                            {
                                switch (childNode.Name)
                                {
                                    case "Base_Value":
                                        int spouseBaseValue = int.Parse(childNode.InnerText);
                                        FTRInitialize.SpouseBaseValue = spouseBaseValue;
                                        break;
                                }
                            }
                        }
                        else
                        {
                            FTRInitialize.EnableSpouse = false;
                        }
                        break;
                    case "Children":
                        if (node.Attributes[0].InnerText.Equals("true"))
                        {
                            FTRInitialize.EnableChildren = true;
                            foreach(XmlNode childNode in node.ChildNodes)
                            {
                                switch (childNode.Name)
                                {
                                    case "Base_Value":
                                        int childrenBaseValue = int.Parse(childNode.InnerText);
                                        FTRInitialize.ChildrenBaseValue = childrenBaseValue;
                                        break;
                                }
                            }
                        }
                        else
                        {
                            FTRInitialize.EnableChildren = false;
                        }
                        break;
                    case "Chatlog_Output":
                        if (node.Attributes[0].InnerText.Equals("true"))
                        {
                            FTRInitialize.EnableChatLogOutput = true;
                            break;
                        }
                        else
                        {
                            FTRInitialize.EnableChatLogOutput = false;
                        }
                        break;
                }
            }
        }
        //Get output from the language xml file
        public void LoadLanguageFile(String languagePath)
        {
            try
            {
                this.lang = XDocument.Load(languagePath);
            }
            catch(Exception e)
            {
                MessageBox.Show("Error while trying to load the language.xml file from 'Fighting Together Relationship' :\n\n" + e.Message + "\n\n" + e.StackTrace);
            }
        }
        public String GetMessage(String node, String childNode)
        {
            LoadLanguageFile(BasePath.Name + "Modules/FightingTogetherRelationship/ModuleData/Language.xml"); 
            String text;
            try
            {
                text = this.lang.Root.Element(node).Element(childNode).Value.ToString();
            }
            catch
            {
                return "Translation could not be found!";
            }
            return text;
        }
    }
}
