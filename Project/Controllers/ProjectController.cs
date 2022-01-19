using Project.Models.DataContext;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using Project.Models.Dtos;

namespace Project.Controllers
{
    public class ProjectController : Controller
    {
        ProjectDBContext dbContext = new ProjectDBContext();
        // GET: Project
        public ActionResult Index()
        {
            var updatedUobList = GetResultList();
            return View(updatedUobList);
        }

        private List<UretimOperasyonBildirimleriOutputDto> GetResultList()
        {
            var uobList = dbContext.UretimOperasyonBildirim.ToList();
            var sdList = dbContext.StandartDuruslar.ToList();

            var final = new List<UretimOperasyonBildirimleriOutputDto>();

            uobList.ForEach(uob => {
                if(sdList.Any(t => uob.Baslangic.TimeOfDay <= t.Baslangic.TimeOfDay && uob.Bitirme.TimeOfDay > t.Baslangic.TimeOfDay)){
                    var standartDurus = sdList
                                            .Where(t => (uob.Baslangic.TimeOfDay <= t.Baslangic.TimeOfDay && uob.Bitirme.TimeOfDay >= t.Baslangic.TimeOfDay)
                                                || (t.Bitirme.TimeOfDay >= uob.Baslangic.TimeOfDay))//uob bitirme <= olamaz çünkü uob patates olur.
                                            .FirstOrDefault();
                    
                    if(uob.Baslangic.TimeOfDay == standartDurus.Baslangic.TimeOfDay)
                    {
                        // Standart Durus kaydını ekle.
                        final.Add(new UretimOperasyonBildirimleriOutputDto()
                        {
                            IsNo = uob.IsNumarasi,
                            Baslangic = standartDurus.Baslangic,
                            Bitis = standartDurus.Bitirme,
                            Sure = standartDurus.Bitirme - standartDurus.Baslangic,
                            Status = "Durus",
                            DurusNedeni = standartDurus.DurusNedeni
                        });
                        if(standartDurus.Bitirme.TimeOfDay != uob.Bitirme.TimeOfDay)
                        {
                            // Kalan UOB kaydını ekle.
                            final.Add(new UretimOperasyonBildirimleriOutputDto()
                            {
                                IsNo = uob.IsNumarasi,
                                Baslangic = standartDurus.Bitirme,
                                Bitis = uob.Bitirme,
                                Sure = uob.Bitirme - standartDurus.Bitirme,
                                Status = uob.Statu,
                                DurusNedeni = uob.DurusNedeni
                            });
                        }
                    }
                    else if(uob.Baslangic.TimeOfDay < standartDurus.Baslangic.TimeOfDay)
                    {
                        final.Add(new UretimOperasyonBildirimleriOutputDto()
                        {
                            IsNo = uob.IsNumarasi,
                            Baslangic =  uob.Baslangic,
                            Bitis = standartDurus.Baslangic,
                            Sure = standartDurus.Baslangic - uob.Baslangic,
                            Status = uob.Statu,
                            DurusNedeni = uob.DurusNedeni
                        });
                        final.Add(new UretimOperasyonBildirimleriOutputDto()
                        {
                            IsNo = uob.IsNumarasi,
                            Baslangic = standartDurus.Baslangic,
                            Bitis = standartDurus.Bitirme,
                            Sure = standartDurus.Bitirme - standartDurus.Baslangic,
                            Status = "Durus",
                            DurusNedeni = standartDurus.DurusNedeni
                        });
                                                                                                                                                                                                                                                                                       
                        if (standartDurus.Bitirme.TimeOfDay != uob.Bitirme.TimeOfDay)
                        {
                            // Kalan UOB kaydını ekle.
                            final.Add(new UretimOperasyonBildirimleriOutputDto()
                            {
                                IsNo = uob.IsNumarasi,
                                Baslangic = standartDurus.Bitirme,
                                Bitis = uob.Bitirme,
                                Sure = uob.Bitirme - standartDurus.Bitirme,
                                Status = uob.Statu,
                                DurusNedeni = uob.DurusNedeni
                            });
                        } 
                    }
                    else if (uob.Baslangic.TimeOfDay > standartDurus.Baslangic.TimeOfDay)
                    {
                        final.Add(new UretimOperasyonBildirimleriOutputDto()
                        {
                            IsNo = uob.IsNumarasi,
                            Baslangic = uob.Baslangic,
                            Bitis = standartDurus.Bitirme,
                            Sure = standartDurus.Bitirme - uob.Baslangic,
                            Status = "Durus",
                            DurusNedeni = standartDurus.DurusNedeni
                        });
                        if (standartDurus.Bitirme.TimeOfDay != uob.Bitirme.TimeOfDay)
                        {
                            // Kalan UOB kaydını ekle.
                            final.Add(new UretimOperasyonBildirimleriOutputDto()
                            {
                                IsNo = uob.IsNumarasi,
                                Baslangic = standartDurus.Bitirme,
                                Bitis = uob.Bitirme,
                                Sure = uob.Bitirme - standartDurus.Bitirme,
                                Status = uob.Statu,
                                DurusNedeni = uob.DurusNedeni
                            });
                        }

                    }
                }
                
                else
                {
                    final.Add(new UretimOperasyonBildirimleriOutputDto()
                    {
                        IsNo = uob.IsNumarasi,
                        Baslangic = uob.Baslangic,
                        Bitis = uob.Bitirme,
                        Sure = uob.Bitirme - uob.Baslangic,
                        Status = uob.Statu,
                        DurusNedeni = uob.DurusNedeni
                    });
                }

            });

            return final;
        }
    }
}