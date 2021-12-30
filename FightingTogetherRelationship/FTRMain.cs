using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;
using System.Windows.Forms;

namespace FightingTogetherRelationship {
    public class FTRMain : MBSubModuleBase
    {
        //Loads the config file on SubModule load
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            String configPath = BasePath.Name + "Modules/FightingTogetherRelationship/ModuleData/FTR_Config.xml";
            String languagePath = BasePath.Name + "Modules/FightingTogetherRelationship/ModuleData/Languages/eng_module_strings.xml";
            FTRConfig.newInstance.LoadConfig(configPath);
            FTRConfig.newInstance.LoadLanguageFile(languagePath);


        }
        //Game startup & information
        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();
            if (!this.gameIsLoaded)
            {
                InformationManager.DisplayMessage(new InformationMessage(FTRConfig.newInstance.GetMessage("Error_Message", "On_Successful_Module_Load"), Color.FromUint(4282569842U)));
                this.gameIsLoaded = true;
            }
        }
        //Load FTR if the save game has been loaded
        public override void OnGameLoaded(Game game, object initializerObject)
        {
            base.OnGameLoaded(game, initializerObject);
            try
            {
                CampaignEvents.OnPlayerBattleEndEvent.AddNonSerializedListener(this, new Action<MapEvent>(FTRConfig.newInstance.FTRCore.CalculateRelation));
            } 
            catch(Exception e)
            {
                MessageBox.Show(FTRConfig.newInstance.GetMessage("Error_Message", "On_Initialize_Error") + " :" + "\n\n" + e.Message + "\n\n" + e.StackTrace);
            }
        }
        bool gameIsLoaded = false;
    }
}
