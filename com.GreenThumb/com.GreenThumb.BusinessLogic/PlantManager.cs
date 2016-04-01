﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.GreenThumb.BusinessObjects;
using com.GreenThumb.DataAccess;

namespace com.GreenThumb.BusinessLogic
{
    //Created by Stenner Kvindlog 
    public class PlantManager
    {


        ///<summary>
        ///Author: Stenner Kvindlog         
        ///fetchPlantList gets a list of all the plants 
        //calling to the plant accessor
        ///Date: 3/4/16
        ///</summary>
        public List<Plant> FetchPlantList(Active active)
        {
            try
            {
                return PlantAccessor.RetrievePlantList(active);
                //return CreateTestPlants(false);
            }
            catch (Exception)
            {
                throw;
            }

        }

        ///<summary>
        ///Author: Stenner Kvindlog         
        ///fetchPlant gets a plant by plantId
        //calling to the plant accessor
        ///Date: 3/4/16
        ///</summary>
        public Plant FetchPlant(int plantId)
        {
            return PlantAccessor.RetrievePlant(plantId);
        }


        ///<summary>
        ///Author: Stenner Kvindlog         
        ///CreatePlant creates a plant 
        //calling to the plant accessor
        ///Date: 3/4/16
        ///</summary>
        public bool CreatePlant(Plant newPlant)
        {
            try
            {
                bool myBool = PlantAccessor.CreatePlant(newPlant);
                return myBool;
            }
            catch (Exception)
            {

                throw;
            }
        }

        ///<summary>
        ///Author: Stenner Kvindlog         
        ///EditPLant sends new and old plant to database to be edited  
        //calling to the plant accessor
        ///Date: 3/4/16
        ///</summary>
        public bool EditPlant(Plant newPlant, Plant oldPlant)
        {
            try
            {
                bool myBool = PlantAccessor.EditPlant(newPlant, oldPlant);
                return myBool;
            }
            catch (Exception)
            {

                throw;
            }

        }


        ///<summary>
        ///Author: Sara Nanke         
        ///Creates some test data 
        ///Date: 3/31/16
        ///</summary>
        public List<Plant> CreateTestPlants(bool IsDB = true)
        {
            List<Plant> plants = new List<Plant>();

            //creating dummy plant list
            var date = new DateTime(1992, 1, 1);
            plants.Add(new
                Plant(null, "Blood Carrot", "Carrot", "Vegetable", "orange for bunnies", "Summer", 1000, date, 1001, date, true));
            plants.Add(new
                Plant(null, "Braburn", "Apple", "Fruit", "sweet crisp and ready to eat", "Summer", 1000, date, 1001, date, true));
            plants.Add(new
                Plant(null, "Michigan Apple", "Apple", "Fruit", "tastes like Michigan", "Summer", 1000, date, 1001, date, true));
            plants.Add(new
                Plant(null, "Pink Lady", "Apple", "Fruit", "best apple ever", "Summer", 1000, date, 1001, date, true));
            plants.Add(new
                Plant(null, "Parsley", "Herb", "Herb", "good on pizza", "Summer", 1000, date, 1001, date, true));
            plants.Add(new
                Plant(null, "Basil", "Herb", "Herb", "good in tomato soup", "Summer", 1000, date, 1001, date, true));

            if (IsDB)
            {
                //creating test plants
                foreach (Plant plant in plants)
                {
                    CreatePlant(plant);
                }
            }
            return plants;
        }

        //(int PlantID, string Name, string Type, string Category, string Description, string Season,
        //              int CreatedBy, DateTime CreatedDate, int ModifiedBy, DateTime ModifiedDate, bool Active)
    }
}
