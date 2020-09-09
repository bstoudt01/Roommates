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

            //GET SINGLE ROOM ENTRY
            Console.WriteLine("----------------------------");
            Console.WriteLine("Getting Room with Id 1");
            Room singleRoom = roomRepo.GetById(1);

            Console.WriteLine($"{singleRoom.Id} {singleRoom.Name} {singleRoom.MaxOccupancy}");

            //ADD ROOM ENTRY
            Room bathroom = new Room
            {
                Name = "Bathroom",
                MaxOccupancy = 1
            };


            roomRepo.Insert(bathroom);

            Console.WriteLine("-------------------------------");
            Console.WriteLine($"Added the new Room with id {bathroom.Id}");


            //UPDATE object above"bathroom" on the property "maxoccupancy" and set it equal to "3"...
            bathroom.MaxOccupancy = 3;
            roomRepo.Update(bathroom);
            //get it from database
            Room bathroomFromDB = roomRepo.GetById(bathroom.Id);
            //..then see it on the console
            Console.WriteLine($"{bathroomFromDB.Id} {bathroomFromDB.Name} {bathroomFromDB.MaxOccupancy}");


            //DELETE ROOM ENTRY BY ID
            roomRepo.Delete(bathroom.Id);
            //get all rooms and loop through them to show its deleted by delting the SQL assigned id of the entry
            allRooms = roomRepo.GetAll();
            foreach(Room room in allRooms){ 
            Console.WriteLine($"{room.Id} {room.Name} {room.MaxOccupancy}");
            }



            /*ROOMMATE TABLE QUERIES
             *************************************************************************************************8
            ROOMMATE TABLE QUERIES */

         //creates new connection to roommate Repo
         RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);

            // GET ALL ROOMMATES
            Console.WriteLine("Getting All Roommates:");
            Console.WriteLine();

            List<Roommate> allRoommates = roommateRepo.GetAll();

            foreach (Roommate roommate in allRoommates)
            {
                Console.WriteLine($"{roommate.Id} {roommate.Lastname}, {roommate.Firstname}");
            }
        
            
            //GET SINGLE Roommate ENTRY
            Console.WriteLine("----------------------------");
            Console.WriteLine("Getting Roommate with Id 1");
            Roommate singleRoommate = roommateRepo.GetById(1);

            Console.WriteLine($"{singleRoommate.Id} {singleRoommate.Firstname}, {singleRoommate.Lastname} {singleRoommate.RentPortion}");


            // GET ALL ROOMmate BY roomId
            Console.WriteLine("Getting All Roommates:");
            Console.WriteLine();

            List<Roommate> roommatesByRoomId = roommateRepo.GetRoommatesByRoomId(3);

            foreach (Roommate roommate in roommatesByRoomId)
            {
                Console.WriteLine($"omgz, {roommate.Id} {roommate.Lastname}, {roommate.Firstname}");
            }


            //ADD ENTRY
            //USED singleRoom that was declared above to be my object represntation that is passed through as an argument to my roommate as the room property
            Roommate bestest  = new Roommate
            {
                Firstname = "Wife",
                Lastname = "#1",
                RentPortion = 50,
                MoveInDate = new DateTime(2011,11,11),
                Room = singleRoom,
            };


            roommateRepo.Insert(bestest);

            Console.WriteLine("-------------------------------");
            Console.WriteLine($"Added the new Roommate id number {bestest.Id} / name: {bestest.Firstname} {bestest.Lastname} / MoveIn: {bestest.MoveInDate} {bestest.Room.Name}");
            



            //UPDATE object above "bestest roommate" on the property "last name" and set it equal to "#2"...
            
            bestest.Lastname = "#2";
            roommateRepo.Update(bestest);
            //get it from database
            Roommate bestestFromDB = roommateRepo.GetById(bestest.Id);
            //..then see it on the console
            Console.WriteLine($"{bestestFromDB.Id} {bestestFromDB.Firstname} {bestestFromDB.Lastname}");


            // DELETE ROOMMATE ENTRY BY ID
            roommateRepo.Delete(bestest.Id);
            //get all roommatess and loop through them to show its deleted by delting the SQL assigned id of the entry
            allRoommates = roommateRepo.GetAll();
            foreach (Roommate roommates in allRoommates)
            {
                Console.WriteLine($"{roommates.Id} {roommates.Lastname} {roommates.Firstname}");
            }

        }
    }
}