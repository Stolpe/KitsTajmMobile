using System;

namespace KitsTajmMobile.Helpers
{
    //http://www.riksdagen.se/sv/Dokument-Lagar/Lagar/Svenskforfattningssamling/Lag-1989253-om-allmanna-hel_sfs-1989-253/
    //https://sv.wikipedia.org/wiki/P%C3%A5skdagen#Ber.C3.A4kning_av_p.C3.A5skdagens_datum
    //Helgdagsafton from https://kitstajm.kits.se/tajm/scripts/scripts.d1aaefbc.js

    public static class HolidayHelper
    {
        public static bool IsHelgdag(DateTime date)
        {
            return IsSunday(date)
                || IsNyårsdagen(date)
                || IsTrettondedagJul(date)
                || IsLångfredagen(date)
                || IsPåskdagen(date)
                || IsAnnandagPåsk(date)
                || IsFörstaMaj(date)
                || IsKristiHimmelsfärdsdag(date)
                || IsPingstdagen(date)
                || IsNationaldagen(date)
                || IsMidsommardagen(date)
                || IsAllaHelgonsDag(date)
                || IsJuldagen(date)
                || IsAnnandagJul(date);
        }

        public static bool IsHelgdagsafton(DateTime date)
        {
            return IsSaturday(date)
                || IsNyårsafton(date)
                || IsMidsommarafton(date)
                || IsJulafton(date);
        }

        #region Is helgdag

        private static bool IsSunday(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Sunday;
        }

        private static bool IsNyårsdagen(DateTime date)
        {
            var nyårsdagen = GetNyårsdagen(date.Year);

            return CompareYearAgnosticDates(date, nyårsdagen);
        }

        private static bool IsTrettondedagJul(DateTime date)
        {
            var trettondedagjul = GetTrettondedagJul(date.Year);

            return CompareYearAgnosticDates(date, trettondedagjul);
        }

        private static bool IsLångfredagen(DateTime date)
        {
            var långfredagen = GetLångfredagen(date.Year);

            return date.Date.Equals(långfredagen.Date);
        }

        private static bool IsPåskdagen(DateTime date)
        {
            var påskdagen = GetPåskdagen(date.Year);

            return date.Date.Equals(påskdagen.Date);
        }

        private static bool IsAnnandagPåsk(DateTime date)
        {
            var annandagpåsk = GetAnnandagPåsk(date.Year);

            return date.Date.Equals(annandagpåsk.Date);
        }

        private static bool IsFörstaMaj(DateTime date)
        {
            var förstamaj = GetFörstaMaj(date.Year);

            return CompareYearAgnosticDates(date, förstamaj);
        }

        private static bool IsKristiHimmelsfärdsdag(DateTime date)
        {
            var kristihimmelfärdsdag = GetKristiHimmelsfärdsdag(date.Year);

            return date.Date.Equals(kristihimmelfärdsdag.Date);
        }

        private static bool IsPingstdagen(DateTime date)
        {
            var pingstdagen = GetPingstdagen(date.Year);

            return date.Date.Equals(pingstdagen.Date);
        }

        private static bool IsNationaldagen(DateTime date)
        {
            var nationaldagen = GetNationaldagen(date.Year);

            return CompareYearAgnosticDates(date, nationaldagen);
        }

        private static bool IsMidsommardagen(DateTime date)
        {
            var midsommardagen = GetMidsommardagen(date.Year);

            return date.Date.Equals(midsommardagen.Date);
        }

        private static bool IsAllaHelgonsDag(DateTime date)
        {
            var allahelgonsdag = GetAllaHelgonsDag(date.Year);

            return date.Date.Equals(allahelgonsdag.Date);
        }

        private static bool IsJuldagen(DateTime date)
        {
            var juldagen = GetJuldagen(date.Year);

            return CompareYearAgnosticDates(date, juldagen);
        }

        private static bool IsAnnandagJul(DateTime date)
        {
            var annandagjul = GetAnnandagJul(date.Year);

            return CompareYearAgnosticDates(date, annandagjul);
        }

        #endregion

        #region Is helgdagsafton

        private static bool IsSaturday(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday;
        }

        private static bool IsNyårsafton(DateTime date)
        {
            var nyårsafton = GetNyårsafton(date.Year);

            return date.Date.Equals(nyårsafton.Date);
        }

        private static bool IsMidsommarafton(DateTime date)
        {
            var midsommarafton = GetMidsommarafton(date.Year);

            return date.Date.Equals(midsommarafton.Date);
        }

        private static bool IsJulafton(DateTime date)
        {
            var julafton = GetJulafton(date.Year);

            return CompareYearAgnosticDates(date, julafton);
        }

        #endregion

        #region Get helgdag

        private static DateTime GetNyårsdagen(int year)
        {
            var nyårsdagen = new DateTime(year, 1, 1);

            return nyårsdagen;
        }

        private static DateTime GetTrettondedagJul(int year)
        {
            var trettondedagjul = new DateTime(year, 1, 6);

            return trettondedagjul;
        }

        private static DateTime GetLångfredagen(int year)
        {
            var långfredagen = GetPåskdagen(year).AddDays(-1);

            while (långfredagen.DayOfWeek.Equals(DayOfWeek.Friday) == false)
            {
                långfredagen = långfredagen.AddDays(-1);
            }

            return långfredagen;
        }

        //disregard dates prior to 1900
        private static DateTime GetPåskdagen(int year)
        {
            var a = year % 19;
            var b = year % 4;
            var c = year % 7;
            var M = 24;
            var d = (19 * a + M) % 30;
            var N = 5;
            var e = (2 * b + 4 * c + 6 * d + N) % 7;

            DateTime påskdagen;

            if (d + e > 9)
            {
                påskdagen = new DateTime(year, 4, d + e - 9);
            }
            else
            {
                påskdagen = new DateTime(year, 3, 22 + d + e);
            }

            if (påskdagen.Date.Equals(new DateTime(year, 4, 26).Date))
            {
                påskdagen = new DateTime(year, 4, 19);
            }
            else if (påskdagen.Date.Equals(new DateTime(year, 4, 25).Date)
                && d == 28
                && e == 6
                && (11 * M + 11) % 30 < 19)
            {
                påskdagen = new DateTime(year, 4, 18);
            }

            return påskdagen;
        }

        private static DateTime GetAnnandagPåsk(int year)
        {
            var annandagpåsk = GetPåskdagen(year).AddDays(1);

            return annandagpåsk;
        }

        private static DateTime GetFörstaMaj(int year)
        {
            var förstamaj =  new DateTime(DateTime.UtcNow.Year, 5, 1);

            return förstamaj;
        }

        private static DateTime GetKristiHimmelsfärdsdag(int year)
        {
            var kristihimmelfärdsdag = GetPåskdagen(year).AddDays(5 * 7 + 1);

            while (kristihimmelfärdsdag.DayOfWeek.Equals(DayOfWeek.Thursday) == false)
            {
                kristihimmelfärdsdag = kristihimmelfärdsdag.AddDays(1);
            }

            return kristihimmelfärdsdag;
        }

        private static DateTime GetPingstdagen(int year)
        {
            var pingstdagen = GetPåskdagen(year).AddDays(6 * 7 + 1);

            while (pingstdagen.DayOfWeek.Equals(DayOfWeek.Sunday) == false)
            {
                pingstdagen = pingstdagen.AddDays(1);
            }

            return pingstdagen;
        }

        private static DateTime GetNationaldagen(int year)
        {
            var nationaldagen = new DateTime(DateTime.UtcNow.Year, 6, 6);

            return nationaldagen;
        }

        private static DateTime GetMidsommardagen(int year)
        {
            var midsommardagen = new DateTime(year, 6, 20);

            while (midsommardagen.DayOfWeek.Equals(DayOfWeek.Saturday) == false)
            {
                midsommardagen = midsommardagen.AddDays(1);
            }

            return midsommardagen;
        }

        private static DateTime GetAllaHelgonsDag(int year)
        {
            var allahelgonsdag = new DateTime(year, 10, 31);

            while (allahelgonsdag.DayOfWeek.Equals(DayOfWeek.Saturday) == false)
            {
                allahelgonsdag = allahelgonsdag.AddDays(1);
            }

            return allahelgonsdag;
        }

        private static DateTime GetJuldagen(int year)
        {
            var juldagen = new DateTime(DateTime.UtcNow.Year, 12, 25);

            return juldagen;
        }

        private static DateTime GetAnnandagJul(int year)
        {
            var annandagjul = GetJuldagen(year).AddDays(1);

            return annandagjul;
        }

        #endregion

        #region Get helgdagsafton

        private static DateTime GetNyårsafton(int year)
        {
            var nyårsafton = GetNyårsdagen(year + 1).AddDays(-1);

            return nyårsafton;
        }

        private static DateTime GetMidsommarafton(int year)
        {
            var midsommarafton = GetMidsommardagen(year).AddDays(-1);

            return midsommarafton;
        }

        private static DateTime GetJulafton(int year)
        {
            var julafton = GetJuldagen(year).AddDays(-1);

            return julafton;
        }

        #endregion

        private static bool CompareYearAgnosticDates(DateTime date1, DateTime date2)
        {
            return date1.Month.Equals(date2.Month)
                && date1.Day.Equals(date2.Day);
        }
    }
}
