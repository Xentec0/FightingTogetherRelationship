using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using System.Windows.Forms;

namespace FightingTogetherRelationship
{
    public class FTRCore
    {
        public void CalculateRelation(MapEvent m)
        {
            if(Hero.MainHero != null && Hero.MainHero.IsAlive)
            {
                if(m.IsFieldBattle || m.IsSiege || m.IsSiegeOutside)
                {
                    if(m.PlayerSide.ToString().Equals("Defender") || m.PlayerSide.ToString().Equals("Attacker"))
                    {
                        MapEventSide sidePlayer = GetPlayerEventSide(m);
                        MapEventSide sideEnemy = GetEnemyEventSide(m);
                        if(sidePlayer != null && sideEnemy != null)
                        {
                            List<PartyBase> allyHeroesB = new List<PartyBase>(sidePlayer.PartiesOnThisSide.ToList<PartyBase>());
                            List<PartyBase> enemyHeroesB = new List<PartyBase>(sideEnemy.PartiesOnThisSide.ToList<PartyBase>());
                            if (allyHeroesB != null && enemyHeroesB != null)
                            {
                                int totalMenAlly = sidePlayer.TroopCount + sidePlayer.Casualties;
                                int totalMenEnemy = sideEnemy.TroopCount + sideEnemy.Casualties;
                                int casualtiesAlly = sidePlayer.Casualties;
                                int casualtiesEnemy = sideEnemy.Casualties;
                                int involvedMen = totalMenAlly + totalMenEnemy;
                                float playerContributionRate = sidePlayer.GetPlayerPartyContributionRate();
                                List <PartyBase> allyHeroes = new List<PartyBase>(ConfigureParties(allyHeroesB));
                                List <PartyBase> enemyHeroes = new List<PartyBase>(ConfigureParties(enemyHeroesB));

                                if (allyHeroes != null && enemyHeroes != null && allyHeroes.Count > 0 && enemyHeroes.Count > 0)
                                {
                                    if (totalMenAlly >= FTRConfig.newInstance.FTRInitialize.MinimumAlly && totalMenEnemy >= FTRConfig.newInstance.FTRInitialize.MinimumEnemy)
                                    {
                                        if (sidePlayer.Equals(m.Winner))
                                        {
                                            bool lostBattle = false;
                                            if (totalMenAlly >= totalMenEnemy)
                                            {
                                                if ((totalMenAlly / totalMenEnemy) <= 1.2 && (totalMenAlly - totalMenEnemy) <= 80)
                                                {
                                                    if (casualtiesEnemy > casualtiesAlly)
                                                    {
                                                        double c1 = 2 + playerContributionRate * 1.3;
                                                        double calcrel = Math.Round(c1, 0, MidpointRounding.AwayFromZero);
                                                        int rel = (int)calcrel;
                                                        double relMulti = rel * FTRConfig.newInstance.FTRInitialize.GainMultiplier;
                                                        rel = (int)Math.Round(relMulti, 0, MidpointRounding.AwayFromZero);
                                                        RelationshipSelector(rel, allyHeroes, enemyHeroes, lostBattle);
                                                    }
                                                    else
                                                    {
                                                        double c2 = 1.25 + playerContributionRate * 1.3;
                                                        double calcrel = Math.Round(c2, 0, MidpointRounding.AwayFromZero);
                                                        int rel = (int)calcrel;
                                                        double relMulti = rel * FTRConfig.newInstance.FTRInitialize.GainMultiplier;
                                                        rel = (int)Math.Round(relMulti, 0, MidpointRounding.AwayFromZero);
                                                        RelationshipSelector(rel, allyHeroes, enemyHeroes, lostBattle);
                                                    }
                                                }
                                                else
                                                {
                                                    if (casualtiesEnemy > casualtiesAlly)
                                                    {
                                                        double c3 = 1.1 + playerContributionRate * 1.3;
                                                        double calcrel = Math.Round(c3, 0, MidpointRounding.AwayFromZero);
                                                        int rel = (int)calcrel;
                                                        double relMulti = rel * FTRConfig.newInstance.FTRInitialize.GainMultiplier;
                                                        rel = (int)Math.Round(relMulti, 0, MidpointRounding.AwayFromZero);
                                                        RelationshipSelector(rel, allyHeroes, enemyHeroes, lostBattle);
                                                    }
                                                    else
                                                    {
                                                        RelationshipSelector(1, allyHeroes, enemyHeroes, lostBattle);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if ((totalMenEnemy / totalMenAlly) >= 1.2 && (totalMenEnemy - totalMenAlly) > 60)
                                                {
                                                    double t = totalMenEnemy / totalMenAlly;
                                                    double c4 = 2.4 + t + (playerContributionRate * 1.3);
                                                    double calcrel = Math.Round(c4, 0, MidpointRounding.AwayFromZero);
                                                    int rel = (int)calcrel;
                                                    double relMulti = rel * FTRConfig.newInstance.FTRInitialize.GainMultiplier;
                                                    rel = (int)Math.Round(relMulti, 0, MidpointRounding.AwayFromZero);
                                                    RelationshipSelector(rel, allyHeroes, enemyHeroes, lostBattle);
                                                }
                                                else
                                                {
                                                    double c5 = 2.25 + (playerContributionRate * 1.3);
                                                    double calcrel = Math.Round(c5, 0, MidpointRounding.AwayFromZero);
                                                    int rel = (int)calcrel;
                                                    double relMulti = rel * FTRConfig.newInstance.FTRInitialize.GainMultiplier;
                                                    rel = (int)Math.Round(relMulti, 0, MidpointRounding.AwayFromZero);
                                                    RelationshipSelector(rel, allyHeroes, enemyHeroes, lostBattle);
                                                }
                                            }
                                        }
                                        if (sidePlayer != m.Winner && sidePlayer.LeaderParty.Equals(PartyBase.MainParty) && FTRConfig.newInstance.FTRInitialize.EnableRelationshipLoss == true)
                                        {
                                            bool lostBattle = true;
                                            if (totalMenAlly >= totalMenEnemy)
                                            {
                                                if (casualtiesAlly <= casualtiesEnemy)
                                                {
                                                    double c6 = playerContributionRate * 1.3;
                                                    c6 = Math.Round(c6, 0, MidpointRounding.AwayFromZero);
                                                    int rel = -2 + (int)c6;
                                                    double relMulti = rel * FTRConfig.newInstance.FTRInitialize.LossMultiplier;
                                                    rel = (int)Math.Ceiling(relMulti);
                                                    RelationshipSelector(rel, allyHeroes, enemyHeroes, lostBattle);
                                                }
                                                else
                                                {
                                                    double c7 = (casualtiesAlly - casualtiesEnemy) / 50;
                                                    c7 = Math.Ceiling(c7);
                                                    double calcrel = playerContributionRate * 1.3;
                                                    calcrel = Math.Round(calcrel, 0, MidpointRounding.AwayFromZero);
                                                    int rel = -2 - (int)c7 + (int)calcrel;
                                                    double relMulti = rel * FTRConfig.newInstance.FTRInitialize.LossMultiplier;
                                                    rel = (int)Math.Ceiling(relMulti);
                                                    RelationshipSelector(rel, allyHeroes, enemyHeroes, lostBattle);
                                                }
                                            }
                                            else
                                            {
                                                if (casualtiesAlly > casualtiesEnemy)
                                                {
                                                    double t1 = (totalMenEnemy - totalMenAlly) / 50;
                                                    double c8 = (casualtiesAlly - casualtiesEnemy) / 50;
                                                    t1 = Math.Ceiling(t1);
                                                    c8 = Math.Ceiling(c8);
                                                    double calcrel = playerContributionRate * 1.3;
                                                    calcrel = Math.Round(calcrel, 0, MidpointRounding.AwayFromZero);
                                                    int rel = -4 - (int)t1 - (int)c8 + (int)calcrel;
                                                    double relMulti = rel * FTRConfig.newInstance.FTRInitialize.LossMultiplier;
                                                    rel = (int)Math.Ceiling(relMulti);
                                                    RelationshipSelector(rel, allyHeroes, enemyHeroes, lostBattle);
                                                }
                                                else
                                                {
                                                    double t2 = (totalMenEnemy - totalMenAlly) / 25;
                                                    t2 = Math.Ceiling(t2);
                                                    double calcrel = playerContributionRate * 1.3;
                                                    calcrel = Math.Round(calcrel, 0, MidpointRounding.AwayFromZero);
                                                    int rel = -4 - (int)t2 + (int)calcrel;
                                                    double relMulti = rel * FTRConfig.newInstance.FTRInitialize.LossMultiplier;
                                                    rel = (int)Math.Ceiling(relMulti);
                                                    RelationshipSelector(rel, allyHeroes, enemyHeroes, lostBattle);

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        //Determains if AI parties relationship will be changed
        private void AddPlayerRelationshipWithLords(int rel, List<PartyBase> allyParties)
        {
            bool isLordParty = true;
            List<String> clans = new List<String>();
            for(int i = 0; i <= allyParties.Count-1; i++)
            {
                if (!clans.Contains(allyParties[i].Owner.Clan.ToString()))
                {
                    clans.Add(allyParties[i].Owner.Clan.ToString());
                }
            }
            //Setting the relation with allied partys leaders in battle
            for(int i = 0; i <= allyParties.Count-1; i++)
            { 
                if (clans != null && !allyParties[i].Owner.Equals(Hero.MainHero))
                {
                    if (clans.Contains(allyParties[i].Owner.Clan.ToString()))
                    {
                        int currentRelation = CharacterRelationManager.GetHeroRelation(Hero.MainHero, allyParties[i].Owner.Clan.Leader);
                        int newRelation = currentRelation + rel;
                        for(int k = 0; k <= allyParties.Count-1; k++)
                        {
                            if (allyParties[i].Owner.Clan.Equals(allyParties[k].Owner.Clan) && !allyParties[k].Owner.Equals(Hero.MainHero))
                            {
                                SetRelation(allyParties[k].Owner, currentRelation, newRelation, rel, isLordParty);
                            }
                        }
                        clans.Remove(allyParties[i].Owner.Clan.ToString());
                    }
                }
            }
        }
        //Determains if companions relationship will be changed
        private void AddPlayerRelationshipWithCompanions(int rel, List<PartyBase> parties, List<PartyBase> enemyParties, bool lostBattle)
        {
            bool isLordParty = false;
            List<Hero> companionsInParty = new List<Hero>(Hero.MainHero.CompanionsInParty.ToList<Hero>());
            List<Hero> clanCompanions = new List<Hero>(Clan.PlayerClan.Companions.ToList<Hero>());
            List<Hero> clanCompanionsRemaining = new List<Hero>(Clan.PlayerClan.Companions.ToList<Hero>());
            //Adds relation to companions if they are in your party and you won
            if (companionsInParty != null && companionsInParty.Count > 0)
                {
                for (int i = 0; i <= companionsInParty.Count-1; i++)
                {
                int currentRelation = CharacterRelationManager.GetHeroRelation(Hero.MainHero, companionsInParty[i]);
                int newRelation = currentRelation + rel;
                clanCompanionsRemaining.Remove(companionsInParty[i]);
                SetRelation(companionsInParty[i], currentRelation, newRelation, rel, isLordParty);
                }
            }
            //Adds relation to companions if they are leading own party in battle
            if (clanCompanionsRemaining.Count > 0)
            {
                for (int i = 0; i <= parties.Count - 1; i++)
                {
                    for (int j = 0; j <= clanCompanions.Count - 1; j++)
                    {
                        if (parties[i].ToString().Equals(clanCompanions[j].ToString() + "'s Party"))
                        {
                            int currentRelation = CharacterRelationManager.GetHeroRelation(Hero.MainHero, clanCompanions[j]);
                            int newRelation = currentRelation + rel;
                            clanCompanionsRemaining.Remove(clanCompanions[j]);
                            SetRelation(clanCompanions[j], currentRelation, newRelation, rel, isLordParty);
                        }
                    }
                }

            }
            //Adds relation to companions if battle is lost and they have been taken prisoner
            if (clanCompanionsRemaining.Count > 0 && enemyParties.Count > 0 && lostBattle == true)
            {
                for(int i = 0; i <= enemyParties.Count - 1; i++)
                {
                    if (!enemyParties[i].Equals(PartyBase.MainParty))
                    {
                        enemyParties[i].Owner.SyncLastSeenInformation();
                        for (int j = 0; j <= clanCompanionsRemaining.Count - 1; j++)
                        {
                            clanCompanionsRemaining[j].SyncLastSeenInformation();
                            if (enemyParties[i].Owner.GetMapPoint().Equals(clanCompanionsRemaining[j].GetMapPoint()))
                            {
                                int currentRelation = CharacterRelationManager.GetHeroRelation(Hero.MainHero, clanCompanionsRemaining[j]);
                                int newRelation = currentRelation + rel;
                                SetRelation(clanCompanionsRemaining[j], currentRelation, newRelation, rel, isLordParty);
                            }
                        }
                    }
                }
            }
        }
        //Determains if spouse relationship will be changed
        private void AddPlayerRelationshipWithSpouse(int rel, List<PartyBase> parties, List<PartyBase> enemyParties, bool lostBattle)
        {
            bool setSpouse = false;
            bool isLordParty = false;
            //Adds relation to spouse if they are in your party and you won
            if (PartyBase.MainParty.MemberRoster.Contains(Hero.MainHero.Spouse.CharacterObject))
            {
                int currentRelation = CharacterRelationManager.GetHeroRelation(Hero.MainHero, Hero.MainHero.Spouse);
                int newRelation = currentRelation + rel;
                setSpouse = true;
                SetRelation(Hero.MainHero.Spouse, currentRelation, newRelation, rel, isLordParty);
            }
            //Adds relation to spouse if they are leading own party in battle
            if(setSpouse != true)
            {
                for (int i = 0; i <= parties.Count - 1; i++)
                {
                    if (parties[i].ToString().Equals(Hero.MainHero.Spouse.ToString() + "'s Party"))
                    {
                        int currentRelation = CharacterRelationManager.GetHeroRelation(Hero.MainHero, Hero.MainHero.Spouse);
                        int newRelation = currentRelation + rel;
                        setSpouse = true;
                        SetRelation(Hero.MainHero.Spouse, currentRelation, newRelation, rel, isLordParty);
                    }
                }
            }
            //Adds relation to spouse if battle is lost and they have been taken prisoner
            if (setSpouse != true && enemyParties.Count > 0 && lostBattle == true)
            {
                Hero.MainHero.Spouse.SyncLastSeenInformation();
                for(int i = 0; i <= enemyParties.Count - 1; i++)
                {
                    if (!enemyParties[i].Equals(PartyBase.MainParty))
                    {
                        enemyParties[i].LeaderHero.SyncLastSeenInformation();
                        if (Hero.MainHero.Spouse.GetMapPoint().Equals(enemyParties[i].LeaderHero.GetMapPoint()))
                        {
                            int currentRelation = CharacterRelationManager.GetHeroRelation(Hero.MainHero, Hero.MainHero.Spouse);
                            int newRelation = currentRelation + rel;
                            SetRelation(Hero.MainHero.Spouse, currentRelation, newRelation, rel, isLordParty);
                        }
                    }

                }
            }
        }
        //Determains if childrens relationship will be changed
        private void AddPlayerRelationshipWithChildren(int rel, List<PartyBase> parties, List<PartyBase> enemyParties, bool lostBattle)
        {
            bool isLordParty = false;
            List<Hero> children = new List<Hero>(Hero.MainHero.Children);
            List<Hero> childrenRemaining = new List<Hero>(Hero.MainHero.Children);

            //Adds relation to children if they are in your party and you won
            for (int i = 0; i <= children.Count-1; i++)
            {
                if (!children[i].IsChild && PartyBase.MainParty.MemberRoster.Contains(children[i].CharacterObject))
                {
                    int currentRelation = CharacterRelationManager.GetHeroRelation(Hero.MainHero, children[i]);
                    int newRelation = currentRelation + rel;
                    childrenRemaining.Remove(children[i]);
                    SetRelation(children[i], currentRelation, newRelation, rel, isLordParty);
                }
            }
            //Adds relation to children if they are leading own party in battle
            if(childrenRemaining.Count > 0)
            {
                for (int i = 0; i <= parties.Count - 1; i++)
                {
                    for (int k = 0; i <= children.Count - 1; i++)
                    {
                        if (!children[i].IsChild && parties[i].ToString().Equals(children[k].ToString()+ "'s Party"))
                        {
                            int currentRelation = CharacterRelationManager.GetHeroRelation(Hero.MainHero, children[k]);
                            int newRelation = currentRelation + rel;
                            childrenRemaining.Remove(children[k]);
                            SetRelation(children[k], currentRelation, newRelation, rel, isLordParty);
                        }
                    }
                }
            }
            //Adds relation to children if battle is lost and they have been taken prisoner
            if(childrenRemaining.Count > 0 && enemyParties.Count > 0 && lostBattle == true)
            {
                for(int i = 0; i <= enemyParties.Count - 1; i++)
                {
                    if (!enemyParties[i].Equals(PartyBase.MainParty))
                    {
                        enemyParties[i].LeaderHero.SyncLastSeenInformation();
                        for (int j = 0; j <= childrenRemaining.Count - 1; j++)
                        {
                            if (!childrenRemaining[j].IsChild)
                            {
                                childrenRemaining[j].SyncLastSeenInformation();
                                if (enemyParties[i].LeaderHero.GetMapPoint().Equals(childrenRemaining[j].GetMapPoint()))
                                {
                                    int currentRelation = CharacterRelationManager.GetHeroRelation(Hero.MainHero, childrenRemaining[j]);
                                    int newRelation = currentRelation + rel;
                                    SetRelation(childrenRemaining[j], currentRelation, newRelation, rel, isLordParty);
                                }
                            }
                        }
                    }
                }
            }
        }
        //Selects added relationship based on config
        private void RelationshipSelector(int rel, List<PartyBase> allyParties, List<PartyBase> enemyParties, bool lostBattle)
        {
            AddPlayerRelationshipWithLords(rel, allyParties);
            if(FTRConfig.newInstance.FTRInitialize.EnableCompanions == true)
            {
                int relC = rel + FTRConfig.newInstance.FTRInitialize.CompanionBaseValue;
                AddPlayerRelationshipWithCompanions(relC, allyParties, enemyParties, lostBattle);
            }
            if(FTRConfig.newInstance.FTRInitialize.EnableSpouse == true && Hero.MainHero.Spouse != null)
            {
                int relS = rel + FTRConfig.newInstance.FTRInitialize.SpouseBaseValue;
                AddPlayerRelationshipWithSpouse(relS, allyParties, enemyParties, lostBattle);
            }
            if(FTRConfig.newInstance.FTRInitialize.EnableChildren == true && Hero.MainHero.Children != null && Hero.MainHero.Children.Count > 0)
            {
                int relK = rel + FTRConfig.newInstance.FTRInitialize.ChildrenBaseValue;
                AddPlayerRelationshipWithChildren(relK, allyParties, enemyParties, lostBattle);
            }
        }
        //Adds the relation to the detrmained allies
        private void SetRelation(Hero alliedHero, int currentRelation, int newRelation, int rel, bool isLordParty)
        {
            int relationCap = FTRConfig.newInstance.FTRInitialize.RelationCap;
            int relToCap;
            if(relationCap > currentRelation)
            {
                if(relationCap >= newRelation)
                {
                    CharacterRelationManager.SetHeroRelation(Hero.MainHero, alliedHero, newRelation);
                    if(isLordParty == true)
                    {
                        CharacterRelationManager.SetHeroRelation(Hero.MainHero, alliedHero.Clan.Leader, newRelation);
                    }
                    RelationshipOutput(rel, newRelation, alliedHero.ToString());
                } 
                else
                {
                    if(rel >= (relationCap - currentRelation))
                    {
                        relToCap = relationCap - currentRelation;
                        newRelation = currentRelation + relToCap;
                        CharacterRelationManager.SetHeroRelation(Hero.MainHero, alliedHero, newRelation);
                        if(isLordParty == true)
                        {
                            CharacterRelationManager.SetHeroRelation(Hero.MainHero, alliedHero.Clan.Leader, newRelation);
                        }
                        RelationshipOutput(relToCap, newRelation, alliedHero.ToString());
                    }
                }
            }
        }
        //Outputs the relationship change
        private void RelationshipOutput(int rel, int newRelation, String hero)
        {
            bool ChatLogOutput = FTRConfig.newInstance.FTRInitialize.EnableChatLogOutput;
            String display = "Error";
            Color col = new Color(1000,1000,1000);
            if(rel < 0)
            {
                display = FTRConfig.newInstance.GetMessage("Relation_Message", "On_Relation_Loss");
                MBTextManager.SetTextVariable("DECREASED_VALUE", rel);
                MBTextManager.SetTextVariable("NEW_RELATION_VALUE", newRelation);
                MBTextManager.SetTextVariable("HERO", hero);
                col = new Color(10000, 1000, 1000);
            }
            if(rel > 0)
            {
                display = FTRConfig.newInstance.GetMessage("Relation_Message", "On_Relation_Gain");
                MBTextManager.SetTextVariable("INCREASED_VALUE", rel);
                MBTextManager.SetTextVariable("NEW_RELATION_VALUE", newRelation);
                MBTextManager.SetTextVariable("HERO", hero);
                col = Color.FromUint(4282569842U);
            }
            if(ChatLogOutput == false)
            {
                TextObject popUpText = new TextObject(display, null);
                InformationManager.AddQuickInformation(popUpText, 0, null, "");
            }
            if(ChatLogOutput == true)
            {
                InformationManager.DisplayMessage(new InformationMessage(display, col));
            }

        }
        //Gets Player EventSide
        private MapEventSide GetPlayerEventSide(MapEvent m)
        {
            if (m.PlayerSide.ToString().Equals("Defender"))
            {
                return m.DefenderSide;
            }
            if (m.PlayerSide.ToString().Equals("Attacker"))
            {
                return m.AttackerSide;
            }
            else
            {
                return null;
            }
        }
        //Gets Enemy EventSide
        private MapEventSide GetEnemyEventSide(MapEvent m)
        {
            if (m.PlayerSide.ToString().Equals("Defender"))
            {
                return m.AttackerSide;
            }
            if (m.PlayerSide.ToString().Equals("Attacker"))
            {
                return m.DefenderSide;
            }
            else
            {
                return null;
            }
        }
        private List<PartyBase> ConfigureParties(List<PartyBase> party)
        {
            List<PartyBase> fixedParty = new List<PartyBase>();
            if (!party.Contains(PartyBase.MainParty)){
                fixedParty.Add(PartyBase.MainParty);
            }
            for(int i = 0; i <= party.Count-1; i++)
            {
                if(!party[i].IsSettlement)
                {
                    if(party[i].MobileParty != null)
                    {
                        if(!(party[i].MobileParty.IsBandit || party[i].MobileParty.IsBanditBossParty || party[i].MobileParty.IsCaravan || party[i].MobileParty.IsDeserterParty || party[i].MobileParty.IsGarrison || party[i].MobileParty.IsLeaderless || !party[i].MobileParty.IsLordParty))
                        {
                            fixedParty.Add(party[i]);
                        }
                    }
                }
            }
            return fixedParty;
        }
    }
}
