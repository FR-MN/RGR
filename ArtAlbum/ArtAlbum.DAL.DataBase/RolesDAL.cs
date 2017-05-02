using ArtAlbum.DAL.Abstract;
using ArtAlbum.DAL.DataBase.Exceptions;
using ArtAlbum.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlbum.DAL.DataBase
{
    public class RolesDAL : IRolesDAL
    {
        private string connectionString;

        public RolesDAL()
        {
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["ArtAlbumDB"].ConnectionString;
            }
            catch (Exception e)
            {
                throw new ConfigurationFileException("error in configuration file", e);
            }
        }

        public bool AddRole(RoleDTO role)
        {
            if (role.Id == null)
            {
                throw new ArgumentNullException("role id is null");
            }
            foreach (var roleData in GetAllRoles())
            {
                if (roleData.Id == role.Id)
                {
                    throw new ArgumentException("role already exist");
                }
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO Roles(Id, Name) VALUES(@Id, @Name)", connection);
                command.Parameters.AddWithValue("@Id", role.Id);
                command.Parameters.AddWithValue("@Name", role.Name);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        public bool AddRoleToUser(Guid userId, Guid roleId)
        {
            if (userId == null || roleId == null)
            {
                throw new ArgumentNullException("one of the relation ids are null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO UsersRoles(userId,roleId) VALUES(@userId, @roleId)", connection);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@roleId", roleId);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        public RoleDTO GetRoleById(Guid roleId)
        {
            if (roleId == null)
            {
                throw new ArgumentNullException("role id is null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Id,Name FROM Roles WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", roleId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    return new RoleDTO()
                    {
                        Id = (Guid)reader["Id"],
                        Name = (string)reader["Name"]
                    };
                }
                throw new NotFoundDataException("role not found");
            }
        }

        public IEnumerable<RoleDTO> GetAllRoles()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Id,Name FROM Roles", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return new RoleDTO()
                    {
                        Id = (Guid)reader["Id"],
                        Name = (string)reader["Name"]
                    };
                }
            }
        }

        public IEnumerable<Guid> GetRolesIdsByUserId(Guid userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT userId,roleId FROM UsersRoles WHERE userId=@userId", connection);
                command.Parameters.AddWithValue("@userId", userId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return (Guid)reader["roleId"];
                }
            }
        }

        public IEnumerable<Guid> GetUsersIdsByRoleId(Guid roleId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT userId,roleId FROM UsersRoles WHERE roleId=@roleId", connection);
                command.Parameters.AddWithValue("@roleId", roleId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return (Guid)reader["userId"];
                }
            }
        }

        public bool RemoveRole(Guid roleId)
        {
            if (roleId == null)
            {
                throw new ArgumentNullException("role id is null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("DELETE FROM Roles WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", roleId);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        public bool RemoveRoleFromUser(Guid userId, Guid roleId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("DELETE FROM UsersRoles WHERE userId=@userId AND roleId=@roleId", connection);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@roleId", roleId);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }
    }
}
