using Server.Items;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using Server.ACC.CSS.Systems.Rogue;
using Server.Items.Crops;
using Server.Regions;

namespace Server.Engines.Quests
{
    public class RogueQuest : BaseQuest
    {
        public RogueQuest()
            : base()
        {
	        AddObjective(new DeliverObjective(typeof(Head),"glowa",10,typeof(CityRegion),"Tasandora"));

	        AddReward(new BaseReward(3060294)); // Coraz blizej opanowania Podstepnych Sztuczek
        }

        public override QuestChain ChainID => QuestChain.Rogue;
        public override Type NextQuest => typeof(RoguePhase2Quest);
        /* Historia o Podstepnym */
        public override object Title => 3060296;
        /*  Chcesz uslyszec pewnie historyje o NIM, taaa? No dobra, to Ci opowiem.
            `Zakapturzona postac przemknela niepostrzezenie w okolice Ogrodow Tasandorskich. Zlecenie bylo
            jasne, czesc wynagrodzenia platna z gory, reszta po skonczonej robocie. Bez pytan, bez wyrzutow
            sumienia. Tak jakby grzyby kozikiem zbieral. Myk i do koszyka - tyle ze glowe zamiast grzyba. Potem
            krotka pogawedka ze Szrama - wszak to on za Tasandore odpowiadal. Nie dziwilo go nawet to, ze
            tym razem zlecenie elfka jakas dala. Nieistotny szczegol. Jesli brzeczaca moneta wchodzila w gre
            posmyrlal by nawet Rikktora po smoczych klejnotach. Ba nawet dla mrocznych elfow gotow robote
            wykonac. Intratne zajecie pozwalalo mu zyc na odpowiednim poziomie. Nie to co inne wyskrobki z
            jego portowej dzielnicy. Dawno zapomnial smrod zepsutych ryb i starych ziemniakow. Zdarzalo mu
            sie jednak zlecenia i w Porcie wykonywac. W tej kwestii gotow na nizsza cene, a w zamian postrach
            budzil. Mozna powiedziec, ze podniecalo go to nawet. Resztki czlowieczenstwa dawno zatracil.
            Jednoczesnie wiedzial, ze pewnego dnia mozliwe ze to on ciemnymi interesami rzadzil bedzie. Droga
            daleka i nie pewna. Za to jaki prestiz. Jeszcze niedawno, jako jeden z nielicznych pokusil sie o
            zdobycie najwiekszego wyroznienia dla zabojcow i rzezimieszkow. Zabil starego straznika
            wieziennego oraz jego kompanow. Nic to, ze ten pies porzadku pilnujacy dziwnym trafem odzyl
            pozniej, byc moze to jakas klatwa albo rzecz zwyczajna w naszym swiecie. Wazne, ze jego glowe
            zdobyl.`. Jak chcesz wiedziec jak nauczyc sie takich sztuczek, to przynies mi 10 lbow.
            
  */
        public override object Description => 3060295;
        /* No wiesz... juz myslalem, ze mi pomozesz. */
        public override object Refuse => 3060185;
        /* A co to? Spekales sie?! Hahaha, no popatrzcie... */
        public override object Uncomplete => 3060297;
        public override bool CanOffer() => true;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class RoguePhase2Quest : BaseQuest
    {
        public RoguePhase2Quest()
            : base()
        {
	        AddObjective(new SlayObjective(typeof(Rzezimieszek), "rzezimieszek", 20));

	        AddReward(new BaseReward(3060294)); // Coraz blizej opanowania Podstepnych Sztuczek
        }

        public override QuestChain ChainID => QuestChain.Rogue;
        public override Type NextQuest => typeof(RoguePhase3Quest);
        /* Droga Eliminacji */
        public override object Title => 3060282;
        /*  Lby lbami, ale Skrytobojca nie samymi zabojstwami zyje... No dobra, sklamalem. Wlasnie dlatego zyje. Jak Ty nie zabijesz, to Ciebie zabija. A zeby Ciebie nie zabili, to zabic Ty ich musisz. Wykos 20 rzezimieszkow, by pokazac im kto jest gosc i kto ma klejnoty wieksze, niz jubiler w Orod.  */ 
        public override object Description => 3060298;
        /* A co to? Spekales sie?! Hahaha, no popatrzcie... */
        public override object Refuse => 3060297;
        /* I jak? Udalo Ci sie zgladzic te monstra?. */
        public override object Uncomplete => 3060284;
        /* Oj, beda z Ciebie ludzie! */
        public override object Complete => 3060285;
        public override bool CanOffer() => true;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class RoguePhase3Quest : BaseQuest
    {
        public RoguePhase3Quest()
            : base()
        {
            AddObjective(new ObtainObjective(typeof(PowderOfTranslocation), "Proszek Translokacji", 50, 0x26B8));
            
            AddReward(new BaseReward(3060294)); // Coraz blizej opanowania Podstepnych Sztuczek
            
        }

        public override QuestChain ChainID => QuestChain.Rogue;
        public override Type NextQuest => typeof(RoguePhase4Quest);
        /* Dostawa dla Szramy */
        public override object Title => 3060300;
        /*  No dobra. Widze, ze na powaznie bierzesz to zadanie. W takim razie podpowiem Ci czego najbardziej nam tu potrzeba. Ostatnio nasze zapasy proszku translokacji skonczyly sie. Szrama zuzyl wszystko do jakichs nowych pulapek *rozklada rece*. Przynies 50 sztuk proszku i zrobimy z Ciebie prawdziwego skrytobujce. */
        public override object Description => 3060299;
        /* No nie. Teraz rezygnujesz?! */
        public override object Refuse => 3060195;
        /* I jak Ci idzie?. */
        public override object Uncomplete => 3060261;
        /* *pokiwal glowa* No, no, dobrze rokujesz. */
        public override object Complete => 3060288;
        public override bool CanOffer() => true;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class RoguePhase4Quest : BaseQuest
    {
        public RoguePhase4Quest()
            : base()
        {

	        AddObjective(new DeliverObjective(typeof(PowerGeneratorKey),"Klucz deszyfrujacy",1,typeof(CityRegion),"Tasandora"));
	        
	        AddReward(new BaseReward(3060294)); // Coraz blizej opanowania Podstepnych Sztuczek
        }

        public override QuestChain ChainID => QuestChain.Rogue;
        public override Type NextQuest => typeof(RoguePhase5Quest);
        /* Jeszcze jedna dostawa */
        public override object Title => 3060289;
        /* Wysmienicie Ci idzie. Tak dobrze, ze poprosze Cie o jeszcze jedna dostawe. Widzisz, nasi wrogowie zaczeli zastawiac pulapki i zakladac zamki na szyfr tu i tam. Potrzebujemy klucz deszyfrujacy, ktory pozwoli zdjac pewne zabezpieczenia. Czy myslisz, ze mozesz sie tym zajac? */
        public override object Description => 3060301;
        /* Ahh, tracisz moj czas, czy cos?. */
        public override object Refuse => 3060261;
        /* I jak Ci idzie? */
        public override object Uncomplete => 3060176;
        /* *podskakuje z radosci* */
        public override object Complete => 3060237;
        public override bool CanOffer() => true;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class RoguePhase5Quest : BaseQuest
    {
        public RoguePhase5Quest()
            : base()
        {
	        AddObjective(new SlayObjective(typeof(PlayerMobile), "dowolne osoby (gracze)", 5));
            
            AddReward(new BaseReward(typeof(RogueFalseCoinScroll), "Falszywa Moneta")); // Falszywa Monet
            AddReward(new BaseReward(typeof(RogueSpellbook), "Księga Podstępnych Sztuczek")); // Księga Podstępnych Sztuczek
        }

        public override QuestChain ChainID => QuestChain.Rogue;
        /* Proba sil */
        public override object Title => 3060291;
        /* Teraz dopiero sprawdzimy czy nadajesz sie na czleka, ktory pojmie prawdziwe sztuczki. Zabij 5 dowolnych celow (graczy), tak, aby nikt Cie nie zauwazyl. Pamietaj, to ostateczna proba  */
        public override object Description => 3060302;
        /* I teraz chcesz mnie opusicic?!. */
        public override object Refuse => 3060180;
        /* I jak Ci idzie? */
        public override object Uncomplete => 3060303;
        /* No i kurrrwancka, o to chodzi! */
        public override object Complete => 3060304;
        public override bool CanOffer() => true;

        public override void GiveRewards()
        {
            Owner.SpecialSkills.Rogue = true;

            base.GiveRewards();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
