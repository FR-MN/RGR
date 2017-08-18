﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtAlbum.UI.Web.Helpers
{
    public static class MapHelper
    {
        private static Dictionary<string, string> countryDic = new Dictionary<string, string>();

        static MapHelper()
        {
            countryDic.Add("Афганистан", "AF");
            countryDic.Add("Аландские острова", "AX");
            countryDic.Add("Албания", "AL");
            countryDic.Add("Алжир", "DZ");
            countryDic.Add("Американское Самоа", "AS");
            countryDic.Add("Андорра", "AD");
            countryDic.Add("Ангилья", "AI");
            countryDic.Add("Антарктида", "AQ");
            countryDic.Add("Антигуа и Барбуда", "AG");
            countryDic.Add("Аргентина", "AR");
            countryDic.Add("Армения", "AM");
            countryDic.Add("Аруба", "AW");
            countryDic.Add("Австралия", "AU");
            countryDic.Add("Австрия", "AT");
            countryDic.Add("Азербайджан", "AZ");
            countryDic.Add("Багамские Острова", "BS");
            countryDic.Add("Бахрейн", "BH");
            countryDic.Add("Бангладеш", "BD");
            countryDic.Add("Барбадос", "BB");
            countryDic.Add("Белоруссия", "BY");
            countryDic.Add("Бельгия", "BE");
            countryDic.Add("Белиз", "BZ");
            countryDic.Add("Бенин", "BJ");
            countryDic.Add("Бермудские Острова", "BM");
            countryDic.Add("Бутан", "BT");
            countryDic.Add("Боливия", "BO");
            countryDic.Add("Бонайре, Синт-Эстатиус и Саба", "BQ");
            countryDic.Add("Босния и Герцеговина", "BA");
            countryDic.Add("Ботсвана", "BW");
            countryDic.Add("Остров Буве", "BV");
            countryDic.Add("Бразилия", "BR");
            countryDic.Add("Британская Территория в Индийском Океане", "IO");
            countryDic.Add("Бруней", "BN");
            countryDic.Add("Болгария", "BG");
            countryDic.Add("Буркина-Фасо", "BF");
            countryDic.Add("Бурунди", "BI");
            countryDic.Add("Камбоджа", "KH");
            countryDic.Add("Камерун", "CM");
            countryDic.Add("Канада", "CA");
            countryDic.Add("Кабо-Верде", "CV");
            countryDic.Add("Острова Кайман", "KY");
            countryDic.Add("Центральноафриканская Республика", "CF");
            countryDic.Add("Чад", "TD");
            countryDic.Add("Чили", "CL");
            countryDic.Add("Китайская Народная Республика", "CN");
            countryDic.Add("Остров Рождества", "CX");
            countryDic.Add("Кокосовые острова", "CC");
            countryDic.Add("Колумбия", "CO");
            countryDic.Add("Коморы", "KM");
            countryDic.Add("Демократическая Республика Конго", "CG");
            countryDic.Add("Конго", "CD");
            countryDic.Add("Острова Кука", "CK");
            countryDic.Add("Коста-Рика", "CR");
            countryDic.Add("Кот-д'Ивуар", "CI");
            countryDic.Add("Хорватия", "HR");
            countryDic.Add("Куба", "CU");
            countryDic.Add("Кюрасао", "CW");
            countryDic.Add("Республика Кипр", "CY");
            countryDic.Add("Чехия", "CZ");
            countryDic.Add("Дания", "DK");
            countryDic.Add("Джибути", "DJ");
            countryDic.Add("Доминика", "DM");
            countryDic.Add("Доминиканская Республика", "DO");
            countryDic.Add("Эквадор", "EC");
            countryDic.Add("Египет", "EG");
            countryDic.Add("Сальвадор", "SV");
            countryDic.Add("Экваториальная Гвинея", "GQ");
            countryDic.Add("Эритрея", "ER");
            countryDic.Add("Эстония", "EE");
            countryDic.Add("Эфиопия", "ET");
            countryDic.Add("Фолклендские острова", "FK");
            countryDic.Add("Фарерские острова", "FO");
            countryDic.Add("Фиджи", "FJ");
            countryDic.Add("Финляндия", "FI");
            countryDic.Add("Франция", "FR");
            countryDic.Add("Французская Гвиана", "GF");
            countryDic.Add("Французская Полинезия", "PF");
            countryDic.Add("Французские Южные и Антарктические территории", "TF");
            countryDic.Add("Габон", "GA");
            countryDic.Add("Гамбия", "GM");
            countryDic.Add("Грузия", "GE");
            countryDic.Add("Германия", "DE");
            countryDic.Add("Гана", "GH");
            countryDic.Add("Гибралтар", "GI");
            countryDic.Add("Греция", "GR");
            countryDic.Add("Гренландия", "GL");
            countryDic.Add("Гренада", "GD");
            countryDic.Add("Гваделупа", "GP");
            countryDic.Add("Гуам", "GU");
            countryDic.Add("Гватемала", "GT");
            countryDic.Add("Гернси", "GG");
            countryDic.Add("Гвинея", "GN");
            countryDic.Add("Гвинея-Бисау", "GW");
            countryDic.Add("Гвиана", "GY");
            countryDic.Add("Республика Гаити", "HT");
            countryDic.Add("Остров Херд и острова Макдональд", "HM");
            countryDic.Add("Ватикан", "VA");
            countryDic.Add("Гондурас", "HN");
            countryDic.Add("Гонконг", "HK");
            countryDic.Add("Венгрия", "HU");
            countryDic.Add("Исландия", "IS");
            countryDic.Add("Индия", "IN");
            countryDic.Add("Индонезия", "ID");
            countryDic.Add("Иран", "IR");
            countryDic.Add("Ирак", "IQ");
            countryDic.Add("Ирландия", "IE");
            countryDic.Add("Остров Мэн", "IM");
            countryDic.Add("Израиль", "IL");
            countryDic.Add("Италия", "IT");
            countryDic.Add("Ямайка", "JM");
            countryDic.Add("Япония", "JP");
            countryDic.Add("Джерси", "JE");
            countryDic.Add("Иордания", "JO");
            countryDic.Add("Казахстан", "KZ");
            countryDic.Add("Кения", "KE");
            countryDic.Add("Кирибати", "KI");
            countryDic.Add("Корея", "KR");
            countryDic.Add("Кувейт", "KW");
            countryDic.Add("Киргизия", "KG");
            countryDic.Add("Лаос", "LA");
            countryDic.Add("Латвия", "LV");
            countryDic.Add("Ливан", "LB");
            countryDic.Add("Лесото", "LS");
            countryDic.Add("Либерия", "LR");
            countryDic.Add("Ливия", "LY");
            countryDic.Add("Лихтенштейн", "LI");
            countryDic.Add("Литва", "LT");
            countryDic.Add("Люксембург", "LU");
            countryDic.Add("Макао", "MO");
            countryDic.Add("Республика Македония", "MK");
            countryDic.Add("Мадагаскар", "MG");
            countryDic.Add("Малави", "MW");
            countryDic.Add("Малайзия", "MY");
            countryDic.Add("Мальдивы", "MV");
            countryDic.Add("Мали", "ML");
            countryDic.Add("Мальта", "MT");
            countryDic.Add("Маршалловы Острова", "MH");
            countryDic.Add("Мартиника", "MQ");
            countryDic.Add("Мавритания", "MR");
            countryDic.Add("Маврикий", "MU");
            countryDic.Add("Майотта", "YT");
            countryDic.Add("Мексика", "MX");
            countryDic.Add("Федеративные Штаты Микронезии", "FM");
            countryDic.Add("Молдавия", "MD");
            countryDic.Add("Монако", "MC");
            countryDic.Add("Монголия", "MN");
            countryDic.Add("Черногория", "ME");
            countryDic.Add("Монсеррат", "MS");
            countryDic.Add("Марокко", "MA");
            countryDic.Add("Мозамбик", "MZ");
            countryDic.Add("Мьянма", "MM");
            countryDic.Add("Намибия", "NA");
            countryDic.Add("Науру", "NR");
            countryDic.Add("Непал", "NP");
            countryDic.Add("Нидерланды", "NL");
            countryDic.Add("Новая Каледония", "NC");
            countryDic.Add("Остров Южный (Новая Зеландия)", "NZ");
            countryDic.Add("Остров Северный (Новая Зеландия)", "NZ");
            countryDic.Add("Никарагуа", "NI");
            countryDic.Add("Нигер", "NE");
            countryDic.Add("Нигерия", "NG");
            countryDic.Add("Ниуэ", "NU");
            countryDic.Add("Остров Норфолк", "NF");
            countryDic.Add("Северные Марианские Острова", "MP");
            countryDic.Add("Норвегия", "NO");
            countryDic.Add("Оман", "OM");
            countryDic.Add("Пакистан", "PK");
            countryDic.Add("Палау", "PW");
            countryDic.Add("Палестина", "PS");
            countryDic.Add("Панама", "PA");
            countryDic.Add("Папуа — Новая Гвинея", "PG");
            countryDic.Add("Парагвай", "PY");
            countryDic.Add("Перу", "PE");
            countryDic.Add("Филиппины", "PH");
            countryDic.Add("Острова Питкэрн", "PN");
            countryDic.Add("Польша", "PL");
            countryDic.Add("Португалия", "PT");
            countryDic.Add("Пуэрто-Рико", "PR");
            countryDic.Add("Катар", "QA");
            countryDic.Add("Реюньон", "RE");
            countryDic.Add("Румыния", "RO");
            countryDic.Add("Россия", "RU");
            countryDic.Add("Руанда", "RW");
            countryDic.Add("Сен-Бартелеми", "BL");
            countryDic.Add("Остров Святой Елены", "SH");
            countryDic.Add("Сент-Китс и Невис", "KN");
            countryDic.Add("Сент-Люсия", "LC");
            countryDic.Add("Сен-Мартен", "MF");
            countryDic.Add("Сен-Пьер и Микелон", "PM");
            countryDic.Add("Сент-Винсент и Гренадины", "VC");
            countryDic.Add("Самоа", "WS");
            countryDic.Add("Сан-Марино", "SM");
            countryDic.Add("Сан-Томе и Принсипи", "ST");
            countryDic.Add("Саудовская Аравия", "SA");
            countryDic.Add("Сенегал", "SN");
            countryDic.Add("Сербия", "RS");
            countryDic.Add("Сейшельские Острова", "SC");
            countryDic.Add("Сьерра-Леоне", "SL");
            countryDic.Add("Сингапур", "SG");
            countryDic.Add("Синт-Мартен", "SX");
            countryDic.Add("Словакия", "SK");
            countryDic.Add("Словения", "SI");
            countryDic.Add("Соломоновы Острова", "SB");
            countryDic.Add("Сомали", "SO");
            countryDic.Add("Южно-Африканская Республика", "ZA");
            countryDic.Add("Южная Георгия и Южные Сандвичевы Острова", "GS");
            countryDic.Add("Южный Судан", "SS");
            countryDic.Add("Испания", "ES");
            countryDic.Add("Шри-Ланка", "LK");
            countryDic.Add("Судан", "SD");
            countryDic.Add("Суринам", "SR");
            countryDic.Add("Шпицберген и Ян-Майен", "SJ");
            countryDic.Add("Свазиленд", "SZ");
            countryDic.Add("Швеция", "SE");
            countryDic.Add("Швейцария", "CH");
            countryDic.Add("Сирия", "SY");
            countryDic.Add("Китайская Республика", "TW");
            countryDic.Add("Таджикистан", "TJ");
            countryDic.Add("Танзания", "TZ");
            countryDic.Add("Таиланд", "TH");
            countryDic.Add("Восточный Тимор", "TL");
            countryDic.Add("Того", "TG");
            countryDic.Add("Токелау", "TK");
            countryDic.Add("Тонга", "TO");
            countryDic.Add("Тринидад и Тобаго", "TT");
            countryDic.Add("Тунис", "TN");
            countryDic.Add("Турция", "TR");
            countryDic.Add("Туркмения", "TM");
            countryDic.Add("Теркс и Кайкос", "TC");
            countryDic.Add("Тувалу", "TV");
            countryDic.Add("Уганда", "UG");
            countryDic.Add("Украина", "UA");
            countryDic.Add("Объединённые Арабские Эмираты", "AE");
            countryDic.Add("Великобритания", "GB");
            countryDic.Add("Соединённые Штаты Америки", "US");
            countryDic.Add("Внешние малые острова США", "UM");
            countryDic.Add("Уругвай", "UY");
            countryDic.Add("Узбекистан", "UZ");
            countryDic.Add("Вануату", "VU");
            countryDic.Add("Венесуэла", "VE");
            countryDic.Add("Суматра", "ID");
            countryDic.Add("Хонсю", "JP");
            countryDic.Add("Восточная Антарктида", "AQ");
        }

        public static IEnumerable<string> GetAllCountryNames()
        {
            return countryDic.Keys.OrderBy(countryName => countryName);
        }

        public static string GetCountryCodeByName(string countryName)
        {
            string countryCode;
            if (!string.IsNullOrWhiteSpace(countryName) && countryDic.TryGetValue(countryName, out countryCode))
            {
                return countryCode;
            }
            return null;
        }

        public static string GetCountryNameByCode(string countryCode)
        {
            if (!string.IsNullOrWhiteSpace(countryCode))
            {
                string countryCodeTemp;
                foreach (var countryName in countryDic.Keys)
                {
                    if (countryDic.TryGetValue(countryName, out countryCodeTemp) && countryCodeTemp == countryCode)
                    {
                        return countryName;
                    }
                }
            }
            return "";
        }
    }
}