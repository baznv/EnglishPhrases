using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishPhrases
{
    public static class Transcript
    {
        public static Dictionary<string, string> dictTranscript;

        static Transcript()
        {
            dictTranscript = new Dictionary<string, string>();
            dictTranscript.Add("\u00F0", "Звонкий согласный звук, средний между \"в\" и \"з\" (пример: brother)"); //ð
            dictTranscript.Add("\u03B8", "Глухой согласный звук, средний между \"ф\" и \"с\" (пример: mouth"); //θ
            dictTranscript.Add("\u02A7", "\"ч\"	(пример: match)"); //ʧ
            dictTranscript.Add("\u02A4", "Звонкое\"ч\", похоже на \"джь\"	magic"); //ʤ
            dictTranscript.Add("\u0292", "Мягкое \"ж\"	pleasure"); //ʒ
            dictTranscript.Add("\u0283", "Мягкое \"ш\"	short"); //ʃ
            dictTranscript.Add("\u014B", "Носовое \"н\"	long"); //ŋ
            dictTranscript.Add("j", "\"й\"	stay"); //j
            dictTranscript.Add("w", "Краткое \"у\", без голоса	water"); //w
            dictTranscript.Add("p", "\"п\"	put"); //p
            dictTranscript.Add("b", "\"б\"	big"); //b
            dictTranscript.Add("m", "\"м\"	mouse"); //m
            dictTranscript.Add("f", "\"ф\"	foot"); //f
            dictTranscript.Add("v", "\"в\"	love"); //v
            dictTranscript.Add("s", "\"с\"	same"); //s
            dictTranscript.Add("z", "\"з\"	zebera"); //z
            dictTranscript.Add("t", "\"т\"	ticket"); //t
            dictTranscript.Add("d", "\"д\"	delete"); //d
            dictTranscript.Add("n", "\"н\"	never"); //n
            dictTranscript.Add("l", "\"л\"	live"); //l
            dictTranscript.Add("r", "Похоже на \"р\" rage"); //r
            dictTranscript.Add("k", "\"к\"	cost"); //k
            dictTranscript.Add("g", "\"г\"	game"); //g
            dictTranscript.Add("h", "\"х\" на выдохе hair"); //h
            dictTranscript.Add("\u026A", "Короткое \"и\" ring"); //ɪ
            dictTranscript.Add("\u0254", "Короткое \"о\" long"); //ɔ
            dictTranscript.Add("\u0251", "Длинное \"а\"	harm"); //ɑ
            dictTranscript.Add("\u028C", "Короткое \"а\"	luck"); //ʌ
            dictTranscript.Add("\u00E6", "Гласный звук, среднее между \"э\" и \"а\" (пример: apple)"); //æ
            dictTranscript.Add("\u0259", "Гласный безударный звук	silver"); //ə
            dictTranscript.Add("\u0259" + ":", ""); //ə:
            dictTranscript.Add("i:", "\"и\"	people"); //i:
            dictTranscript.Add("e", "\"э\"	take"); //e
            dictTranscript.Add("u", "\"у\"	put"); //u
            dictTranscript.Add("u:", "Длинное \"у\"	move"); //u:
            dictTranscript.Add("o:", "Длинное \"о\"	all"); //o:
            dictTranscript.Add("ju:", "Дифтонг \"ью\"	new"); //ju:
            dictTranscript.Add("ei", "Дифтонг \"эй\"	make"); //ei
            dictTranscript.Add("\u0251" + "i", "Дифтонг \"ай\"	like"); //ɑi
            dictTranscript.Add("\u0251" + "u", "Дифтонг \"ау\"	mouse"); //ɑu
            dictTranscript.Add("\u0254" + "i", "Дифтонг \"ой\"	boy"); //ɔi
            dictTranscript.Add("\u0254" + "u", "Дифтонг \"оу\"	home"); //ɔu
            dictTranscript.Add("i" + "\u0259", "Дифтонг, похожий на \"иэ\"	ear"); //iə
            dictTranscript.Add("\u025B" + "\u0259", "Дифтонг, похожий на \"эе\"	air"); //ɛə
            dictTranscript.Add("u" + "\u0259", "Дифтонг, похожий на \"уе\" poor"); //uə
            dictTranscript.Add("j" + "u" + "\u0259", "Трифтонг, похожий на \"йуе\"	Europe"); //juə
            dictTranscript.Add("\u0251" + "i" + "\u0259", "Трифтонг похожий на короткое \"аие\"	fire"); //ɑiə
            dictTranscript.Add("\u0251" + "u" + "\u0259", "Трифтонг, похожий на \"ауэ\"	hour"); //ɑuə
            dictTranscript.Add("`", "Главное ударение"); //`
            dictTranscript.Add("\u02CF", "Вторичное ударение"); //

        }
    }
}
