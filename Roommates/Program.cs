using System;
using System.Collections.Generic;
using Roommates.Models;
using Roommates.Repositories;

namespace Roommates
{
    class Program
    {
        /// <summary>
        ///  This is the address of the database.
        ///  We define it here as a constant since it will never change.
        /// </summary>
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);

            //GET ALL ROOMS
            Console.WriteLine("Getting All Rooms:");
            Console.WriteLine();

            List<Room> allRooms = roomRepo.GetAll();

            foreach (Room room in allRooms)
            {
                Console.WriteLine($"{room.Id} {room.Name} {room.MaxOccupancy}");
            }

            //GET SINGLE ENTRY
            Console.WriteLine("----------------------------");
            Console.WriteLine("Getting Room with Id 1");
            Room singleRoom = roomRepo.GetById(1);

            Console.WriteLine($"{singleRoom.Id} {singleRoom.Name} {singleRoom.MaxOccupancy}");

            //ADD ENTRY
            Room bathroom = new Room
            {
                Name = "Bathroom",
                MaxOccupancy = 1
            };


            roomRepo.Insert(bathroom);

            Console.WriteLine("-------------------------------");
            Console.WriteLine($"Added the new Room with id {bathroom.Id}");


            //UPDATE object above"bathroom" on the property "maxoccupency" and set it equal to "3"...
            bathroom.MaxOccupancy = 3;
            roomRepo.Update(bathroom);
            //get it from database
            Room bathroomFromDB = roomRepo.GetById(bathroom.Id);
            //..then see it on the console
            Console.WriteLine($"{bathroomFromDB.Id} {bathroomFromDB.Name} {bathroomFromDB.MaxOccupancy}");


            //DELETE ENTRY BY ID
            roomRepo.Delete(bathroom.Id);
            //get all rooms and loop through them to show its deleted by delting the SQL assigned id of the entry
            allRooms = roomRepo.GetAll();
            foreach(Room room in allRooms){ 
            Console.WriteLine($"{room.Id} {room.Name} {room.MaxOccupancy}");
            }

        }
    }
}