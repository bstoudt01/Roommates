using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using Roommates.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace Roommates.Repositories
{
    ///<summary>
    ///  This class is responsible for interacting with Roommate data.
    ///  It inherits from the BaseRepository class so that it can use the BaseRepository's Connection property
    /// </summary>
    class RoommateRepository : BaseRepository
    {
        /// <summary>
        ///  When new RoomRespository is instantiated, pass the connection string along to the BaseRepository
        /// </summary>
        public RoommateRepository(string connectionString) : base(connectionString) { }

        // ...We'll add some methods shortly...
        /// <summary>
        ///  Get a list of all Rooms in the database
        /// </summary>
        public List<Roommate> GetAll()
        {
            //  We must "use" the database connection.
            //  In C#, a "using" block ensures we correctly disconnect from a resource even if there is an error.
            //  For database connections, this means the connection will be properly closed.
            using (SqlConnection conn = Connection)
            {
                // Note, we must Open() the connection, the "using" block doesn't do that for us.
                conn.Open();

                // We must "use" commands too.
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // Here we setup the command with the SQL we want to execute before we execute it.
                    cmd.CommandText = "SELECT Id, Firstname, Lastname, RentPortion, MoveInDate, RoomId FROM Roommate";

                    // Execute the SQL in the database and get a "reader" that will give us access to the data.
                    SqlDataReader reader = cmd.ExecuteReader();

                    // A list to hold the rooms we retrieve from the database.
                    List<Roommate> roommates = new List<Roommate>();

                    // Read() will return true if there's more data to read
                    while (reader.Read())
                    {
                        // The "ordinal" is the numeric position of the column in the query results.
                        //  For our query, "Id" has an ordinal value of 0 and "Name" is 1.
                        int idColumnPosition = reader.GetOrdinal("Id");

                        // We user the reader's GetXXX methods to get the value for a particular ordinal.
                        int idValue = reader.GetInt32(idColumnPosition);

                        int FirstnameColumnPosition = reader.GetOrdinal("Firstname");
                        string FirstnameValue = reader.GetString(FirstnameColumnPosition);

                        int LastnameColumnPosition = reader.GetOrdinal("Lastname");
                        string LastnameValue = reader.GetString(LastnameColumnPosition);

                        int RentPortionColumnPosition = reader.GetOrdinal("RentPortion");
                        int RentPortionValue = reader.GetInt32(RentPortionColumnPosition);


                        int MoveInDateColumnPosition = reader.GetOrdinal("MoveInDate");
                        DateTime MoveInDateValue = reader.GetDateTime(MoveInDateColumnPosition);

                        int Room = reader.GetOrdinal("RoomId");
                           //initally being set as null inside the object so it does not need to be declared outside of it.
                        // Now let's create a new room object using the data from the database.
                        Roommate roommate = new Roommate
                        {
                            Id = idValue,
                            Firstname = FirstnameValue,
                            Lastname = LastnameValue,
                            RentPortion = RentPortionValue,
                            MoveInDate = MoveInDateValue,
                            Room = null,

                        };

                        // ...and add that room object to our list.
                        roommates.Add(roommate);
                    }


                    // We should Close() the reader. Unfortunately, a "using" block won't work here.
                    reader.Close();

                    // Return the list of rooms who whomever called this method.
                    return roommates;
                }
            }
        }

        /// <summary>
        ///  Returns a single room with the given id.
        /// </summary>
        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Firstname, Lastname, RentPortion, MoveInDate, RoomId FROM Roommate WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;

                    // If we only expect a single row back from the database, we don't need a while loop.
                    if (reader.Read())
                    {
                        roommate = new Roommate
                        {
                            Id = id,
                            Firstname = reader.GetString(reader.GetOrdinal("Firstname")),
                            Lastname = reader.GetString(reader.GetOrdinal("Lastname")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),

                         
                            MoveInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                            Room = null,
                    };
                    }

                    reader.Close();

                    return roommate;
                }
            }
        }

        //public List<Roommate> GetRoommatesByRoomId(int roomId)
        //Roommate objects should have a Room property
        public List<Roommate> GetRoommatesByRoomId(int roomId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Firstname, Lastname, RentPortion, MoveInDate, RoomId FROM Roommate WHERE roomId = @roomId";
                    cmd.Parameters.AddWithValue("@roomId", roomId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    // If we only expect a single row back from the database, we don't need a while loop.
                    // A list to hold the rooms we retrieve from the database.
                    List<Roommate> GetRoommatesByRoomId = new List<Roommate>();

                    // Read() will return true if there's more data to read
                    while (reader.Read())
                    {
                        // The "ordinal" is the numeric position of the column in the query results.
                        //  For our query, "Id" has an ordinal value of 0 and "Name" is 1.
                        int idColumnPosition = reader.GetOrdinal("Id");

                        // We user the reader's GetXXX methods to get the value for a particular ordinal.
                        int idValue = reader.GetInt32(idColumnPosition);

                        int FirstnameColumnPosition = reader.GetOrdinal("Firstname");
                        string FirstnameValue = reader.GetString(FirstnameColumnPosition);

                        int LastnameColumnPosition = reader.GetOrdinal("Lastname");
                        string LastnameValue = reader.GetString(LastnameColumnPosition);

                        int RentPortionColumnPosition = reader.GetOrdinal("RentPortion");
                        int RentPortionValue = reader.GetInt32(RentPortionColumnPosition);


                        int MoveInDateColumnPosition = reader.GetOrdinal("MoveInDate");
                        DateTime MoveInDateValue = reader.GetDateTime(MoveInDateColumnPosition);

                        int Room = reader.GetOrdinal("RoomId");
                        //initally being set as null inside the object so it does not need to be declared outside of it.
                        // Now let's create a new room object using the data from the database.
                        Roommate roommate = new Roommate
                        {
                            Id = idValue,
                            Firstname = FirstnameValue,
                            Lastname = LastnameValue,
                            RentPortion = RentPortionValue,
                            MoveInDate = MoveInDateValue,
                            Room = null,

                        };

                        // ...and add that room object to our list.
                        GetRoommatesByRoomId.Add(roommate);
                    }


                    // We should Close() the reader. Unfortunately, a "using" block won't work here.
                    reader.Close();

                    // Return the list of rooms who whomever called this method.
                    return GetRoommatesByRoomId;
                }
            }
        }
    }
}
