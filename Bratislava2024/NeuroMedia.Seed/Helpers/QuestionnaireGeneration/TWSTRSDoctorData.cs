using NeuroMedia.Application.Features.Questionnaires.Dtos;
using NeuroMedia.Application.Features.Questionnaires.Helpers;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Seed.Helpers.QuestionnaireGeneration
{
    public class TWSTRSDoctorData : IQuestionnaireGeneration
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
                    Text = "Rotácia (horizontálne otáčanie: doprava alebo doľava). Rotácia je definovaná ako pohyb hlavy pozdĺž horizontálnej osi. Pohyb brady od stredovej čiary doprava alebo doľava je najlepšie viditeľný pri čelnom pohľade. V strednej polohe je brada umiestnená priamo nad hrudnou kosťou, uprostred medzi spojením kľúčnych kostí. Rotácia sa hodnotí podľa najväčšieho stupňa vychýlenia od stredu polohy." ,
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
                            Text = "Ľahká (menej ako 25 % plného rozsahu; 1 - 22 stupňov od stredovej línie)",
                            Value = "1"
                        },
                        new OptionDto
                        {
                            Id = 3,
                            Text = "Mierna (25 až menej ako 50 % plného rozsahu; 23 - 45 stupňov od stredovej čiary)",
                            Value = "2"
                        },
                        new OptionDto
                        {
                            Id = 4,
                            Text = "Stredne ťažká (50 až menej ako 75 % plného rozsahu; 46 - 67 stupňov od stredovej čiary)",
                            Value = "3"
                        },
                        new OptionDto
                        {
                            Id = 5,
                            Text = "Ťažká (75 % alebo viac plného rozsahu; 68 - 90 stupňov od stredovej čiary)",
                            Value = "4"
                        }
                    ]
                },
                new QuestionDto
                {
                    Id = 2,
                    Type = "Radio",
                    Text = "Laterokolis (náklon vpravo alebo vľavo, vylúčenie elevácie ramena). Laterokolis sa vzťahuje na uhol naklonenie hlavy doprava alebo doľava, ale nezahŕňa eleváciu ramien. Rovnako ako pri rotácii, maximálna odchýlka v bočnom smere je skóre, ktoré sa má zaznamenať. Technika na určenie sklonu hlavy je nakreslenie čiary medzi očami alebo ušami a porovnanie tejto čiary s horizontálnou rovinou.",
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
                            Text = "Ľahká (menej ako 25 % plného rozsahu; 1 - 22 stupňov od stredovej čiary)",
                            Value = "1"
                        },
                        new OptionDto
                        {
                            Id = 3,
                            Text = "Mierna (25 až menej ako 50 % plného rozsahu; 23 - 45 stupňov od stredovej čiary)",
                            Value = "2"
                        },
                        new OptionDto
                        {
                            Id = 4,
                            Text = "Stredne ťažká (50 až menej ako 75 % plného rozsahu; 46 - 67 stupňov od stredovej čiary)",
                            Value = "3"
                        },
                        new OptionDto
                        {
                            Id = 5,
                            Text = "Ťažká (75 % alebo viac plného rozsahu; 68 - 90 stupňov od stredovej čiary)",
                            Value = "4"
                        }
                    ]
                },
                new QuestionDto
                {
                    Id = 3,
                    Type = "Radio",
                    Text = "Elevácia ramena/predný posun. Táto kategória zahŕňa hodnotenie závažnosti pohybu ramena, ako aj faktor trvania pohybu ramena. Elevácia ramena sa najlepšie hodnotí z čelného alebo zadného pohľadu. Predný alebo zadný posun ramena sa najlepšie hodnotí z bočného alebo profilového pohľadu.",
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
                            Text = "Ľahká (< 25 % plného rozsahu) prerušované alebo trvalé",
                            Value = "1"
                        },
                        new OptionDto
                        {
                            Id = 3,
                            Text = "Mierna (25 % až menej ako 50 % plného rozsahu) prerušované alebo stále",
                            Value = "2"
                        },
                        new OptionDto
                        {
                            Id = 4,
                            Text = "Stredne ťažká (50 % až menej ako 75 % plného rozsahu) prerušovaný alebo stály",
                            Value = "3"
                        },
                        new OptionDto
                        {
                            Id = 5,
                            Text = "Ťažká (75 % alebo viac plného rozsahu) prerušovaný alebo stály",
                            Value = "4"
                        }
                    ]
                },
                new QuestionDto
                {
                    Id = 4,
                    Type = "Radio",
                    Text = "Trvanie cervikálnej dystónie počas vyšetrenia. Trvanie cervikálnej dystónie sa určuje počas celého vyšetrenia a je to hodnotenie pohybu hlavy v akomkoľvek smere. Skladá sa z dvoch zložiek: (a) percento času, počas ktorého je počas celého vyšetrenia prítomná odchýlka hlavy a b) relatívna intenzita odchýlky hlavy počas vyšetrenia (napr. ak bola odchýlka hlavy prítomná počas vyšetrenia, bola najčastejšie submaximálne alebo maximálne prítomná). Všimnite si, že trvanie pohybu ramien sa v tejto kategórii neberie do úvahy, ale hodnotí sa nižšie v inej časti.",
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
                            Text = "Príležitostná odchýlka (menej ako 25 % času), maximálna alebo submaximálna ",
                            Value = "1"
                        },
                        new OptionDto
                        {
                            Id = 3,
                            Text = "Občasná odchýlka (25 - 50 % času), buď maximálna alebo submaximálna ALEBO Častá odchýlka (50 - 75 % času), najčastejšie submaximálna 2",
                            Value = "2"
                        },
                        new OptionDto
                        {
                            Id = 4,
                            Text = "Častá odchýlka (50 - 75 % času), najčastejšie maximálna ALEBO Stála odchýlka (viac ako 75 % času), najčastejšie submaximálna 3",
                            Value = "3"
                        },
                        new OptionDto
                        {
                            Id = 5,
                            Text = "Stála odchýlka (viac ako 75 % času), najčastejšie maximálna 4",
                            Value = "4"
                        }
                    ]
                },
                new QuestionDto
                {
                    Id = 5,
                    Type = "Radio",
                    Text = "Rozsah pohybu hlavy a krku. Kategória rozsahu pohybu hodnotí schopnosť pohybovať sa z abnormálnej polohy cez stredovú líniu do krajnej opačnej polohy bez pomoci senzorických trikov. Rozsah pohybu sa hodnotí pre každú z troch osí pohybu hlavy: horizontálna rotácia, flexia/extenzia a bočný náklon. Skóre pre najzávažnejšie obmedzenie smeru pohybu je konečné skóre rozsahu pohybu.",
                    Options = [
                        new OptionDto
                        {
                            Id = 1,
                            Text = "Schopnosť pohybu do krajnej opačnej polohy ",
                            Value = "0"
                        },
                        new OptionDto
                        {
                            Id = 2,
                            Text = "Schopnosť pohybovať hlavou ďaleko za stredovú čiaru, ale nie do extrémnej opačnej polohy ",
                            Value = "1"
                        },
                        new OptionDto
                        {
                            Id = 3,
                            Text = "Schopnosť pohybovať hlavou minimálne za stredovú čiaru ",
                            Value = "2"
                        },
                        new OptionDto
                        {
                            Id = 4,
                            Text = "Schopnosť pohybovať hlavou smerom k stredovej čiare, ale nie za ňu ",
                            Value = "3"
                        },
                        new OptionDto
                        {
                            Id = 5,
                            Text = "Sotva schopný pohnúť hlavou mimo abnormálnej polohy ",
                            Value = "4"
                        }
                    ]
                },
                new QuestionDto
                {
                    Id = 6,
                    Type = "Radio",
                    Text = "Čas držania hlavy v stredovej línii. Táto položka hodnotí schopnosť pacienta držať hlavu v rozsahu 10 stupňov od stredovej čiary, normálnej polohy hlavy. Získanie stredovej polohy sa môže uskutočniť slovným pokynom. Získanie stredovej polohy znamená začiatok merania času. Schopnosť zotrvať v stredovej línii sa zisťuje dvakrát a priemerné trvanie do 60 sekúnd pri každom pokuse sa spriemeruje, aby sa získalo skóre. Ak pacient nedokáže dosiahnuť stredovú líniu, skóre je 4.",
                    Options = [
                        new OptionDto
                        {
                            Id = 1,
                            Text = "> 60 sekúnd 0",
                            Value = "0"
                        },
                        new OptionDto
                        {
                            Id = 2,
                            Text = "46 - 60 sekúnd 1",
                            Value = "1"
                        },
                        new OptionDto
                        {
                            Id = 3,
                            Text = "31 - 45 sekúnd 2",
                            Value = "2"
                        },
                        new OptionDto
                        {
                            Id = 4,
                            Text = "6 - 30 sekúnd 3",
                            Value = "3"
                        },
                        new OptionDto
                        {
                            Id = 5,
                            Text = "< 15 sekúnd 4",
                            Value = "4"
                        }
                    ]
                },
            ]
        };
    }
}
