using System;
using System.Collections.Generic;
//using System.Text;
using LibraryConfigUtilities;
using System.Globalization;
using System.Linq;




namespace LibraryBusiness
{
   
    public class PenaltyFeeCalculator{
        
        private List<Country> SettingList; //her bir ülke için tanımlı Country nesnelerini tutan liste
       
        
        public List<Country> GetCountryList() //Windows Formun ülkeleri çekebilmesi için
        {
            return SettingList;
        }

        public PenaltyFeeCalculator() 
        {
            var settings = new LibrarySetting(); //LibrarySetting nesnesini alıp settingList’e atama
            SettingList = settings.LibrarySettingList;
        }
        
        public int GetBusinessDays(DateTime start, DateTime end,Country country)
        {
            // iş günlerini hesaplayan kod buraya gelecek
            
            
            int businessDays = 0;
            
            List<DateTime> holiday = country.HolidayList;  // Ekstra tatil günleri
            List<DayOfWeek> weekend = country.WeekendList; // Hafta sonu günleri
            
            for (DateTime date = start; date <= end; date = date.AddDays(1)) // Günleri baştan sonra gezip kontrol ediyor
            {
                
                if (country.WeekendList.Contains(date.DayOfWeek))  // Hafta sonu kontrolü
                    continue;
                
                
                if (holiday != null && holiday.Contains(date)) // Tatil günü kontrolü
                    continue;
                
                businessDays++; // Hafta sonu veya tatil değilse + iş günü
            }
            return businessDays;
        }

        
        
        public String Calculate(DateTime start, DateTime end, string cultureCode){
            
            if (start > end)
          {
              return $"Hatalı giriş: Başlangıç tarihi bitiş tarihinden sonra olamaz.";
          }
           
            Country country = SettingList.FirstOrDefault(c => c.CountryCode.Equals(cultureCode, StringComparison.OrdinalIgnoreCase));
            // cultureCode a göre ülke bul
            
            if (country == null)
                return $"Ülke bulunamadı: {cultureCode}"; 
            
            int totalBusinessDays = GetBusinessDays(start, end, country); // Toplam iş günü hesapla
            int lateDays = totalBusinessDays - country.PenaltyAppliesAfter; // Toplam iş gününden, ceza kaç gün sonra başlıyorsa çıkar

            if (lateDays <= 0)
                return $"0 /Ceza yok ({totalBusinessDays} iş günü)";
            // else
            
            decimal fee = lateDays * country.DailyPenaltyFee;
            CultureInfo culture = new CultureInfo(country.Culture);
            return fee.ToString("C", culture);
        }
        
        
    }
}
