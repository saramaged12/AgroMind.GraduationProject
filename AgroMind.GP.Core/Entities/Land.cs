using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Entities
{
	public class Land
	{
        public Land(int id)
        {
            Id = id;
        }

        public int Id { get; set; }

        public string name { get; set; }

        public string location { get; set; }

        public double areaSize { get; set; }
        public string unitOfMeasurement { get; set; }


        public string soilType { get; set; }

        public string waterSource { get; set; }

        public string type { get; set; }

        public string image { get; set; }

        //public List<Crop> cropsTypes { get; set; } = new List<Crop>();
        public List<Crop> cropsTypes { get; set; }

        [ForeignKey("FarmerId")]   //??????????????
        public Farmer ownerShip { get; set; }

        public string status { get; set; }

        public string weatherCondition { get; set; }

        //public List<string> history { get; set; } = new List<string>();
        public List<string> history { get; set; }

        public string currentCrop { get; set; }

        public bool isDeleted { get; set; }

        //areaSizeInM2 ----> mean????? and difference between areaSize and areaSizeInM2?????
        public double areaSizeInM2 { get; set; }
    }

    public class Crop
    {
        public int Id { get; set; }   //id of crop
        public string name { get; set; }
    }

    public class Farmer
    {
        public int Id { get; set; }  //id of farmer
        public string name { get; set; }
    }

}

