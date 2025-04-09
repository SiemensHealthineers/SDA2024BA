using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuroMedia.Application.Features.Questionnaires.Dtos;
using NeuroMedia.Application.Features.Questionnaires.Helpers;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Seed.Helpers.QuestionnaireGeneration
{
    public class TWSTRSPatientQuestionnaireData : IQuestionnaireGeneration
    {
        public QuestionnaireDto QuestionnaireDto => s_questionnaire;
        private static readonly QuestionnaireDto s_questionnaire = new()
        {
            BlobPath = $"{QuestionnaireHelper.QuestionnaireFolder}/{QuestionnaireType.TWSTRSPatient}{QuestionnaireHelper.QuestionnaireFileExt}",
            Questions = [
                new QuestionDto
                {
                    Id = 1,
                    Type = "Radio",
                    Text = "Práca (zamestnanie alebo domáce práce/správanie domácnosti).",
                    Options = [
                        new OptionDto
                        {
                            Id = 1,
                            Text = "Bez ťažkostí",
                            Value = "0"
                        },
                        new OptionDto
                        {
                            Id = 2,
                            Text = "Bežné pracovné očakávania s uspokojivým výkonom na bežnej úrovni povolania, ale určitá interferencia v dôsledku tortikolis",
                            Value = "1"
                        },
                        new OptionDto
                        {
                            Id = 3,
                            Text = "Väčšina činností neobmedzená, vybrané činnosti veľmi ťažké a sťažené, ale stále možné s uspokojivým výkonom",
                            Value = "2"
                        },
                        new OptionDto
                        {
                            Id = 4,
                            Text = "Práca na nižšej úrovni, ako je zvyčajná úroveň povolania; väčšina činností sťažená, všetky možné, ale v niektorých činnostiach menej ako uspokojivý výkon.",
                            Value = "3"
                        },
                        new OptionDto
                        {
                            Id = 5,
                            Text = "Nemožnosť vykonávať dobrovoľnú alebo zárobkovú činnosť; stále schopný vykonávať niektoré domáce povinnosti uspokojivo",
                            Value = "4"
                        },
                        new OptionDto
                        {
                            Id = 6,
                            Text = "Okrajová alebo žiadna schopnosť vykonávať domáce povinnosti",
                            Value = "5"
                        }
                    ]
                },
                new QuestionDto
                {
                    Id = 2,
                    Type = "Radio",
                    Text = "Činnosti každodenného života (napr. kŕmenie, obliekanie, hygiena (vrátane umývania, holenia, líčenia atď.)",
                    Options = [
                        new OptionDto
                        {
                            Id = 1,
                            Text = "Žiadne ťažkosti so žiadnou činnosťou",
                            Value = "0"
                        },
                        new OptionDto
                        {
                            Id = 2,
                            Text = "Aktivity bez obmedzenia, ale s určitými prekážkami spôsobenými dystóniou",
                            Value = "1"
                        },
                        new OptionDto
                        {
                            Id = 3,
                            Text = "Väčšina činností neobmedzená, vybrané činnosti veľmi ťažké a sťažené, ale stále možné pomocou jednoduchých trikov",
                            Value = "2"
                        },
                        new OptionDto
                        {
                            Id = 4,
                            Text = "Väčšina činností sťažená alebo namáhavá, ale stále možná; môže používať extrémne „triky“",
                            Value = "3"
                        },
                        new OptionDto
                        {
                            Id = 5,
                            Text = "Všetky činnosti sťažené; niektoré nemožné alebo si vyžadujú pomoc",
                            Value = "4"
                        },
                        new OptionDto
                        {
                            Id = 6,
                            Text = "Odkázaný na pomoc iných pri väčšine sebaobslužných úkonov",
                            Value = "5"
                        }
                    ]
                },
                new QuestionDto
                {
                    Id = 3,
                    Type = "Radio",
                    Text = "Šoférovanie",
                    Options = [
                        new OptionDto
                        {
                            Id = 1,
                            Text = "Bez ťažkostí (alebo nikdy nejazdil autom) ",
                            Value = "0"
                        },
                        new OptionDto
                        {
                            Id = 2,
                            Text = "Neobmedzená schopnosť šoférovať, ale obťažujúca dystonia ",
                            Value = "1"
                        },
                        new OptionDto
                        {
                            Id = 3,
                            Text = "Neobmedzená schopnosť šoférovať, ale vyžaduje si „triky“ (vrátane dotýkania sa alebo držania tváre; držanie hlavy o opierku hlavy) na ovládanie dystónie",
                            Value = "2"
                        },
                        new OptionDto
                        {
                            Id = 4,
                            Text = "Môže šoférovať len na krátke vzdialenosti",
                            Value = "3"
                        },
                        new OptionDto
                        {
                            Id = 5,
                            Text = "Zvyčajne nemôže šoférovať kvôli dystónii",
                            Value = "4"
                        },
                        new OptionDto
                        {
                            Id = 6,
                            Text = "Nemôže šoférovať a nemôže jazdiť v aute dlhé úseky ako spolujazdec kvôli dystónii",
                            Value = "5"
                        }
                    ]
                },
                new QuestionDto
                {
                    Id = 4,
                    Type = "Radio",
                    Text = "Čítanie",
                    Options = [
                        new OptionDto
                        {
                            Id = 1,
                            Text = "Bez ťažkostí",
                            Value = "0"
                        },
                        new OptionDto
                        {
                            Id = 2,
                            Text = "Neobmedzená schopnosť čítať v normálnej polohe v sede, ale obťažujúca dystónia",
                            Value = "1"
                        },
                        new OptionDto
                        {
                            Id = 3,
                            Text = "Neobmedzená schopnosť čítať v normálnej polohe v sede, ale vyžaduje si používanie „trikov“ na ovládanie dystónie",
                            Value = "2"
                        },
                        new OptionDto
                        {
                            Id = 4,
                            Text = "Neobmedzená schopnosť čítať, ale vyžaduje si rozsiahle opatrenia na kontrolu dystónie alebo je schopný len čítať v polohe, ktorá nie je v sede (napr. v ľahu)",
                            Value = "3"
                        },
                        new OptionDto
                        {
                            Id = 5,
                            Text = "Obmedzená schopnosť čítať kvôli dystónii napriek trikom",
                            Value = "4"
                        },
                        new OptionDto
                        {
                            Id = 6,
                            Text = "Neschopnosť čítať viac ako niekoľko viet kvôli dystónii",
                            Value = "5"
                        }
                    ]
                },
                new QuestionDto
                {
                    Id = 5,
                    Type = "Radio",
                    Text = "Sledovanie televízie",
                    Options = [
                        new OptionDto
                        {
                            Id = 1,
                            Text = "Bez ťažkostí",
                            Value = "0"
                        },
                        new OptionDto
                        {
                            Id = 2,
                            Text = "Neobmedzená schopnosť sledovať televíziu v normálnej polohe v sede, ale obťažujúca dystónia",
                            Value = "1"
                        },
                        new OptionDto
                        {
                            Id = 3,
                            Text = "Neobmedzená schopnosť sledovať televíziu v normálnej polohe v sede, ale vyžaduje si používanie „trikov“ na ovládanie dystónie",
                            Value = "2"
                        },
                        new OptionDto
                        {
                            Id = 4,
                            Text = "Neobmedzená schopnosť sledovať televíziu, ale vyžaduje si rozsiahle opatrenia na kontrolu dystónie alebo sa dá len pozerať v polohe, keď sa nesedí (napr. v ľahu)",
                            Value = "3"
                        },
                        new OptionDto
                        {
                            Id = 5,
                            Text = "Obmedzená schopnosť sledovať televíziu kvôli dystónii",
                            Value = "4"
                        },
                        new OptionDto
                        {
                            Id = 6,
                            Text = "Nemožnosť sledovať televíziu dlhšie ako niekoľko minút kvôli dystónii",
                            Value = "5"
                        }
                    ]
                },
                new QuestionDto
                {
                    Id = 6,
                    Type = "Radio",
                    Text = "Aktivity mimo domova (napr. nakupovanie, prechádzky, kino, večera v reśtaurácii alebo iné rekreačné aktivity)",
                    Options = [
                        new OptionDto
                        {
                            Id = 1,
                            Text = "Bez ťažkostí",
                            Value = "0"
                        },
                        new OptionDto
                        {
                            Id = 2,
                            Text = "Neobmedzené aktivity, ale obťažujúca dystónia",
                            Value = "1"
                        },
                        new OptionDto
                        {
                            Id = 3,
                            Text = "Neobmedzené aktivity, ale na ich vykonávanie je potrebné používať „triky“ ",
                            Value = "2"
                        },
                        new OptionDto
                        {
                            Id = 4,
                            Text = "Aktivity vykonáva len v sprievode iných osôb kvôli dystónii",
                            Value = "3"
                        },
                        new OptionDto
                        {
                            Id = 5,
                            Text = "Obmedzené aktivity mimo domova; niektoré aktivity nie sú možné alebo sa ich vzdali kvôli dystónii",
                            Value = "4"
                        },
                        new OptionDto
                        {
                            Id = 6,
                            Text = "Zriedkavo, ak vôbec, vykonáva činnosti mimo domova",
                            Value = "5"
                        }
                    ]
                },
                new QuestionDto
                {
                    Id = 7,
                    Type = "DropDown",
                    Text = "Ohodnoťte intenzitu bolesti krku počas posledného týždňa na stupnici 0 - 10, pričom skóre 1 predstavuje minimálnu bolesť a 10 predstavuje tú najtrýznivejšiu bolesť, akú si možno predstaviť"+
                    "\nNajlepšia bolesť:",
                    Options = [
                        new OptionDto
                        {
                            Id = 0,
                            Text = "0",
                            Value = "0"
                        },
                        new OptionDto
                        {
                            Id = 1,
                            Text = "1",
                            Value = "1"
                        },
                        new OptionDto
                        {
                            Id = 2,
                            Text = "2",
                            Value = "2"
                        },
                        new OptionDto
                        {
                            Id = 3,
                            Text = "3",
                            Value = "3"
                        },
                        new OptionDto {
                            Id = 4,
                            Text = "4",
                            Value = "4"
                        },
                        new OptionDto
                        {
                            Id = 5,
                            Text = "5",
                            Value = "5"
                        },
                        new OptionDto
                        {
                            Id = 6,
                            Text = "6",
                            Value = "6"
                        },
                        new OptionDto
                        {
                            Id = 7,
                            Text = "7",
                            Value = "7"
                        },
                        new OptionDto
                        {
                            Id = 8,
                            Text = "8",
                            Value = "8"
                        },
                        new OptionDto
                        {
                            Id = 9,
                            Text = "9",
                            Value = "9"
                        },
                        new OptionDto
                        {
                            Id = 10,
                            Text = "10",
                            Value = "10"
                        }
                    ]
                },
                new QuestionDto
                {
                    Id = 8,
                    Type = "DropDown",
                    Text = "Ohodnoťte intenzitu bolesti krku počas posledného týždňa na stupnici 0 - 10, pričom skóre 1 predstavuje minimálnu bolesť a 10 predstavuje tú najtrýznivejšiu bolesť, akú si možno predstaviť" +
                    "\nNajhoršia bolesť:",
                    Options = [
                        new OptionDto
                        {
                            Id = 0,
                            Text = "0",
                            Value = "0"
                        },
                        new OptionDto
                        {
                            Id = 1,
                            Text = "1",
                            Value = "1"
                        },
                        new OptionDto
                        {
                            Id = 2,
                            Text = "2",
                            Value = "2"
                        },
                        new OptionDto
                        {
                            Id = 3,
                            Text = "3",
                            Value = "3"
                        },
                        new OptionDto {
                            Id = 4,
                            Text = "4",
                            Value = "4"
                        },
                        new OptionDto
                        {
                            Id = 5,
                            Text = "5",
                            Value = "5"
                        },
                        new OptionDto
                        {
                            Id = 6,
                            Text = "6",
                            Value = "6"
                        },
                        new OptionDto
                        {
                            Id = 7,
                            Text = "7",
                            Value = "7"
                        },
                        new OptionDto
                        {
                            Id = 8,
                            Text = "8",
                            Value = "8"
                        },
                        new OptionDto
                        {
                            Id = 9,
                            Text = "9",
                            Value = "9"
                        },
                        new OptionDto
                        {
                            Id = 10,
                            Text = "10",
                            Value = "10"
                        }
                    ]
                },
                new QuestionDto
                {
                    Id = 9,
                    Type = "DropDown",
                    Text = "Ohodnoťte intenzitu bolesti krku počas posledného týždňa na stupnici 0 - 10, pričom skóre 1 predstavuje minimálnu bolesť a 10 predstavuje tú najtrýznivejšiu bolesť, akú si možno predstaviť" +
                    "\nObvyklá bolesť:",
                    Options = [
                        new OptionDto
                        {
                            Id = 0,
                            Text = "0",
                            Value = "0"
                        },
                        new OptionDto
                        {
                            Id = 1,
                            Text = "1",
                            Value = "1"
                        },
                        new OptionDto
                        {
                            Id = 2,
                            Text = "2",
                            Value = "2"
                        },
                        new OptionDto
                        {
                            Id = 3,
                            Text = "3",
                            Value = "3"
                        },
                        new OptionDto {
                            Id = 4,
                            Text = "4",
                            Value = "4"
                        },
                        new OptionDto
                        {
                            Id = 5,
                            Text = "5",
                            Value = "5"
                        },
                        new OptionDto
                        {
                            Id = 6,
                            Text = "6",
                            Value = "6"
                        },
                        new OptionDto
                        {
                            Id = 7,
                            Text = "7",
                            Value = "7"
                        },
                        new OptionDto
                        {
                            Id = 8,
                            Text = "8",
                            Value = "8"
                        },
                        new OptionDto
                        {
                            Id = 9,
                            Text = "9",
                            Value = "9"
                        },
                        new OptionDto
                        {
                            Id = 10,
                            Text = "10",
                            Value = "10"
                        }
                    ]
                },
                new QuestionDto
                {
                    Id = 10,
                    Type = "Radio",
                    Text = "Zhodnoťte trvanie bolesti krku",
                    Options = [
                        new OptionDto
                        {
                            Id = 1,
                            Text = "Žiadna",
                            Value = "0"
                        },
                        new OptionDto
                        {
                            Id = 2,
                            Text = "Prítomná < 10 % času ",
                            Value = "1"
                        },
                        new OptionDto
                        {
                            Id = 3,
                            Text = "Prítomná 10 % až < 25 % času",
                            Value = "2"
                        },
                        new OptionDto
                        {
                            Id = 4,
                            Text = "Prítomná 25 % až < 50 % času ",
                            Value = "3"
                        },
                        new OptionDto
                        {
                            Id = 5,
                            Text = "Prítomná 50 % až < 75 % času ",
                            Value = "4"
                        },
                        new OptionDto
                        {
                            Id = 6,
                            Text = "Prítomný > 75 % času ",
                            Value = "5"
                        }
                    ]
                },
                new QuestionDto
                {
                    Id = 11,
                    Type = "Radio",
                    Text = "Ohodnoťte, do akej miery bolesť prispieva k zdravotnému postihnutiu",
                    Options = [
                        new OptionDto
                        {
                            Id = 1,
                            Text = "Žiadne obmedzenie alebo rušenie spôsobené bolesťou",
                            Value = "0"
                        },
                        new OptionDto
                        {
                            Id = 2,
                            Text = "Bolesť je dosť obťažujúca, ale nie je zdrojom postihnutia",
                            Value = "1"
                        },
                        new OptionDto
                        {
                            Id = 3,
                            Text = "Bolesť určite zasahuje do niektorých úloh, ale nie je hlavným zdrojom postihnutia",
                            Value = "2"
                        },
                        new OptionDto
                        {
                            Id = 4,
                            Text = "Bolesť spôsobuje niektoré (menej ako polovicu), ale nie všetky zdravotné postihnutia",
                            Value = "3"
                        },
                        new OptionDto
                        {
                            Id = 5,
                            Text = "Bolesť je hlavným zdrojom ťažkostí pri činnostiach; oddelene od nej je dystónia tiež zdrojom určitého (menej ako polovičného) postihnutia",
                            Value = "4"
                        },
                        new OptionDto
                        {
                            Id = 6,
                            Text = "Bolesť je hlavným zdrojom postihnutia; bez nej by bolo možné vykonávať väčšinu sťažených činností celkom uspokojivo aj napriek dystónii",
                            Value = "5"
                        }
                    ]
                },
                new QuestionDto
                {
                    Id = 12,
                    Type = "Radio",
                    Text = "Bolo v poslednom mesiaci obdobie, keď ste sa cítili depresívne alebo na dne?",
                    Options = [
                        new OptionDto
                        {
                            Id = 1,
                            Text = "Vôbec",
                            Value = "0"
                        },
                        new OptionDto
                        {
                            Id = 2,
                            Text = "Ľahko: Príležitostný smútok zodpovedajúci okolnostiam",
                            Value = "1"
                        },
                        new OptionDto
                        {
                            Id = 3,
                            Text = "Stredne ťažko: Smutný, ale bez ťažkostí zlepšiť si náladu",
                            Value = "2"
                        },
                        new OptionDto
                        {
                            Id = 4,
                            Text = "Výrazne: Všadeprítomné pocity smútku alebo skľúčenosti. Nálada je stále ovplyvnená vonkajšími okolnosťami",
                            Value = "3"
                        },
                        new OptionDto
                        {
                            Id = 5,
                            Text = "Veľmi závažne: Trvalý alebo nemenný smútok, nešťastie alebo skľúčenosť",
                            Value = "4"
                        }
                    ]
                },
                new QuestionDto
                {
                    Id = 13,
                    Type = "Radio",
                    Text = "Stratili ste za posledný mesiac záujem alebo potešenie z vecí, ktoré vás zvyčajne bavili? (označte podľa subjektívneho prežívania záujmu na rozdiel od skutočnej schopnosti vykonávať činnosti)",
                    Options = [
                        new OptionDto
                        {
                            Id = 1,
                            Text = "Vôbec: Normálny záujem o okolie a iných ľudí",
                            Value = "0"
                        },
                        new OptionDto
                        {
                            Id = 2,
                            Text = "Mierne: Znížená schopnosť tešiť sa z bežných záujmov, činností, koníčkov, ľudí alebo práce, ale žiadny problém začať činnosti",
                            Value = "1"
                        },
                        new OptionDto
                        {
                            Id = 3,
                            Text = "Stredne: Mierna strata záujmu o činnosti, záľuby, ľudí alebo prácu tak, že je ťažké začať niektoré činnosti",
                            Value = "2"
                        },
                        new OptionDto
                        {
                            Id = 4,
                            Text = "Výrazne: Výrazná strata záujmu o okolie a strata záujmu o pobyt s priateľmi a známych s výraznym znížením iniciovania aktivít",
                            Value = "3"
                        },
                        new OptionDto
                        {
                            Id = 5,
                            Text = "Veľmi výrazne: Trvalá a prakticky nepretržitá strata záujmu o všetky činnosti vrátane spoločenských, aj o aktivity s najbližšími priateľmi a príbuznými: neschopnosť iniciovať aktivity",
                            Value = "4"
                        }
                    ]
                },
                new QuestionDto
                {
                    Id = 14,
                    Type = "Radio",
                    Text = "Báli ste sa za posledný mesiac niečoho urobiť alebo ste cítili, že je nepríjemné robiť činnosti pred inými ľuďmi, napríklad hovoriť, jesť alebo písať?",
                    Options = [
                        new OptionDto
                        {
                            Id = 1,
                            Text = "Vôbec",
                            Value = "0"
                        },
                        new OptionDto
                        {
                            Id = 2,
                            Text = "Ľahko: Úzkosť v niektorých spoločenských situáciách, ale naďalej sa zúčastňujem",
                            Value = "1"
                        },
                        new OptionDto
                        {
                            Id = 3,
                            Text = "Stredne ťažké: Úzkosť vo väčšine spoločenských situácií a vyhýbanie sa niektorým činnostiam, ktoré zahŕňajú veľké množstvo ľudí alebo byť stredobodom pozornosti (napr. dvíhanie prípitku, kladenie otázok na fóre at'd)",
                            Value = "2"
                        },
                        new OptionDto
                        {
                            Id = 4,
                            Text = "Výrazne: Výrazná úzkosť vo väčšine spoločenských situácií a vyhýbanie sa väčšine činností",
                            Value = "3"
                        },
                        new OptionDto
                        {
                            Id = 5,
                            Text = "Závažné: Výrazná úzkosť a vyhýbanie sa všetkým spoločenským situáciám okrem spoločnosti najbližšej rodiny/opatrovateľov",
                            Value = "4"
                        }
                    ]
                },
                new QuestionDto
                {
                    Id = 15,
                    Type = "Radio",
                    Text = "Boli ste v poslednom mesiaci obzvlášť nervózny alebo úzkostlivý?",
                    Options = [
                        new OptionDto
                        {
                            Id = 1,
                            Text = "Vôbec",
                            Value = "0"
                        },
                        new OptionDto
                        {
                            Id = 2,
                            Text = "Ľahko: Trochu viac, ako je potrebné, sa obáva drobných záležitostí, ale len s miernym znepokojením",
                            Value = "1"
                        },
                        new OptionDto
                        {
                            Id = 3,
                            Text = "Mierne: Vtieravé úzkostné myšlienky neprimerané situácii, ale schopný sa ich zbaviť",
                            Value = "2"
                        },
                        new OptionDto
                        {
                            Id = 4,
                            Text = "Výrazne: Nepretržité obavy kolísajú v intenzite, znepokojujúce myšlienky môžu na chvíľu ustať alebo dve hodiny, najmä ak je rozptýlená činnosťou, ktorá si vyžaduje pozornosť.",
                            Value = "3"
                        },
                        new OptionDto
                        {
                            Id = 5,
                            Text = "Závažné: Prakticky neprestávajúci strach alebo úzkosť ",
                            Value = "4"
                        }
                    ]
                },
                new QuestionDto
                {
                    Id = 16,
                    Type = "Radio",
                    Text = "Mali ste za posledný mesiac záchvat paniky, keď ste sa náhle cítili vystrašení alebo sa u vás náhle objavilo veľa telesných príznakov? Bežné fyzické symptómy sú: búšenie srdca, potenie, tras alebo chvenie, pocz nedostatku vzduchu alebo dusenie, bolesť na hrudi, nevoľnosť,závraty/mdloby, trpnutie tela, zimnica alebo návaly horúčavy.",
                    Options = [
                        new OptionDto
                        {
                            Id = 1,
                            Text = "Vôbec",
                            Value = "0"
                        },
                        new OptionDto
                        {
                            Id = 2,
                            Text = "Zriedkavé epizódy (menej ako raz za mesiac) paniky navodené špecifickými spúšťačmi",
                            Value = "1"
                        },
                        new OptionDto
                        {
                            Id = 3,
                            Text = "Najmenej 2 záchvaty paniky za posledný mesiac plus určitá očakávaná úzkosť, bez akéhokoľvek vyhýbavého správania",
                            Value = "2"
                        },
                        new OptionDto
                        {
                            Id = 4,
                            Text = "Aspoň raz týždenne záchvaty paniky a výrazná anticipačná úzkosť (strach z opakovania) a určité vyhýbavé správanie",
                            Value = "3"
                        },
                        new OptionDto
                        {
                            Id = 5,
                            Text = "Závažné: Záchvaty paniky takmer denne plus výrazné obavy z opakovania a výrazné vyhýbavé správanie",
                            Value = "4"
                        }
                    ]
                },
                new QuestionDto
                {
                    Id = 17,
                    Type = "Radio",
                    Text = "Mali ste v poslednom mesiaci strach vyjsť sami z domu, byť v dave ľudí, stáť v rade alebo cestovať autobusom či vlakom?",
                    Options = [
                        new OptionDto
                        {
                            Id = 1,
                            Text = "Vôbec",
                            Value = "0"
                        },
                        new OptionDto
                        {
                            Id = 2,
                            Text = "Ľahko: Určitý nepríjemný pocit v niekoľkých špecifických prostrediach (napr. prednáška, autobusy, verejná doprava)",
                            Value = "1"
                        },
                        new OptionDto
                        {
                            Id = 3,
                            Text = "Stredne ťažko: Vyhýba sa niektorým prostrediam",
                            Value = "2"
                        },
                        new OptionDto
                        {
                            Id = 4,
                            Text = "Výrazne: Vyhýba sa väčšine prostredí",
                            Value = "3"
                        },
                        new OptionDto
                        {
                            Id = 5,
                            Text = "Závažné: Veľmi zriedkavo, ak vôbec, opúšťa domov sám",
                            Value = "4"
                        }
                    ]
                },
            ]
        };
    }
}
