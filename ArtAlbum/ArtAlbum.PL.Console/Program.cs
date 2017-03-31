using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtAlbum.Entities;
using ArtAlbum.BLL.Abstract;
using ArtAlbum.PL.Models;

namespace UsersImages.PL.Console
{
    class Program
    {
        private static IUsersBLL usersBLL;
        private static IImagesBLL imagesBLL;
        private static IUsersImagesBLL relationsBLL;
        static void Main(string[] args)
        {
            try
            {
                //usersBLL = Provaider.UsersBLL;
                //imagesBLL = Provaider.AwardsBLL;
                //relationsBLL = Provaider.RelationsBLL;

                bool succes;
                int menuNumber = 0;
                do
                {
                    do
                    {
                        System.Console.Clear();
                        System.Console.WriteLine(" 1 - Show all users");
                        System.Console.WriteLine(" 2 - Show all images");
                        System.Console.WriteLine(" 3 - Show all specific user images");
                        System.Console.WriteLine(" 4 - Show all users with specific images");
                        System.Console.WriteLine(" 5 - Add new user");
                        System.Console.WriteLine(" 6 - -");
                        System.Console.WriteLine(" 7 - Add images to user");
                        System.Console.WriteLine(" 8 - Remove images from user");
                        System.Console.WriteLine(" 9 - Remove user");
                        System.Console.WriteLine("10 - -");
                        System.Console.WriteLine("11 - Update user");
                        System.Console.WriteLine("12 - Update image");
                        System.Console.WriteLine("13 - Exit");
                        System.Console.Write("Enter the number of menu: ");
                        succes = int.TryParse(System.Console.ReadLine(), out menuNumber);
                        System.Console.WriteLine();
                    } while (!succes);
                    if (menuNumber > 0 && menuNumber < 13)
                    {
                        switch (menuNumber)
                        {
                            case 1: { ShowAllUsers(); } break;
                            case 2: { ShowAllImages(); } break;
                            case 3: { ShowImagesOfUser(); } break;
                            case 4: { ShowUsersWithImages(); } break;
                            case 5: { AddUser(); } break;
                            case 6: {  } break;//я думаю такое делать не надо AddImage();
                            case 7: { AddImageToUser(); } break;
                            case 8: { DeleteImageFromUser(); } break;
                            case 9: { DeleteUser(); } break;
                            case 10: {  } break;//тоже не нада DeleteAward();
                            case 11: { UpdateUser(); } break;
                            case 12: { UpdateImage(); } break;
                        }
                    }
                } while (menuNumber != 13);
            }
            catch
            {
                System.Console.WriteLine("ЧТО ТО НЕ ТАК");
            }
        }
        private static void ShowAllUsers()
        {
            System.Console.WriteLine("List of users:{0}", Environment.NewLine);
            ViewUsersFromContainer(usersBLL.GetAllUsers().OrderBy(user => user.LastName));
            System.Console.ReadLine();
        }
        private static void ViewUsersFromContainer(IEnumerable<UserDTO> container)
        {
            if (container.Count() == 0)
            {
                System.Console.WriteLine("There are no users yet");
            }
            else
            {
                int number = 0;
                foreach (var user in container)
                {
                    System.Console.WriteLine("{0} - {1}", ++number, (UserVM)user);
                }
            }
        }
        private static void ShowAllImages()
        {
            System.Console.WriteLine("List of awards:{0}", Environment.NewLine);
            ViewImagesFromContainer(imagesBLL.GetAllImages().OrderBy(image => image.DateOfCreating));
            System.Console.ReadLine();
        }
        private static void ViewImagesFromContainer(IEnumerable<ImageDTO> container)
        {
            if (container.Count() == 0)
            {
                System.Console.WriteLine("There are no awards yet");
            }
            else
            {
                int number = 0;
                foreach (var image in container)
                {
                    System.Console.WriteLine("{0} - {1}", ++number, (ImageVM)image);// нету оператора преобразвание в самой моделе
                }
            }
        }

        private static void ShowImagesOfUser()// логично сортировать по LastName
        {
            var users = usersBLL.GetAllUsers().OrderBy(user => user.LastName);
            int number = GetSelectedNumberOfUser(users);
            if (number != 0)
            {
                var user = users.Skip(number - 1).First();
                System.Console.WriteLine(((UserVM)user).ToString());
                System.Console.WriteLine("Count of awards of this user: {0}", relationsBLL.GetImagesByUser(user.Id).Count());
                System.Console.WriteLine("List of awards:\n");
                ViewImagesFromContainer(relationsBLL.GetImagesByUser(user.Id).OrderBy(image => image.DateOfCreating));
                System.Console.ReadLine();
            }
        }
        private static int GetSelectedNumberOfUser(IEnumerable<UserDTO> container)
        {
            ViewUsersFromContainer(container);
            int number;
            System.Console.Write("\nEnter number of user: ");
            int.TryParse(System.Console.ReadLine(), out number);
            if (container.Count() >= number && number > 0)
            {
                return number;
            }
            else
            {
                return 0;
            }
        }

        private static void ShowUsersWithImages()
        {
            var images = imagesBLL.GetAllImages().OrderBy(image => image.DateOfCreating);
            int number = GetSelectedNumberOfImage(images);
            if (number != 0)
            {
                var image = images.Skip(number - 1).First();
                System.Console.WriteLine(((ImageVM)image).ToString());
                System.Console.WriteLine("Count of users with this award: {0}", relationsBLL.GetUsersByImage(image.Id).Count());
                System.Console.WriteLine("List of users:\n");
                ViewUsersFromContainer(relationsBLL.GetUsersByImage(image.Id).OrderBy(user => user.LastName));
                System.Console.ReadLine();
            }
        }
        private static int GetSelectedNumberOfImage(IEnumerable<ImageDTO> container)
        {
            ViewImagesFromContainer(container);
            int number;
            System.Console.Write("\nEnter number of award: ");
            int.TryParse(System.Console.ReadLine(), out number);
            if (container.Count() >= number && number > 0)
            {
                return number;
            }
            else
            {
                return 0;
            }
        }

        //Больше ПРОВЕРОК!!
        private static void AddUser()// сюда бы регулярку прикрутить
        {
            System.Console.Write("Registration!!!!! ");

            System.Console.Write("Enter your's Email: ");
            string email = System.Console.ReadLine();

            System.Console.Write("Actually right now We don't have working convector to password  , and you can register without password )))) ");
            //string password = System.Console.ReadLine();

            System.Console.Write("Enter your's Nickname: ");
            string nickname = System.Console.ReadLine();

            System.Console.Write("Enter your's First Name: ");
            string firstname = System.Console.ReadLine();

            System.Console.Write("Enter your's Last Name: ");
            string lastname = System.Console.ReadLine();

            System.Console.Write("Enter your's date of birth (dd/mm/yyyy): ");
            DateTime dateOfBirth;
            // проверки
            if (string.IsNullOrWhiteSpace(nickname) | !DateTime.TryParse(System.Console.ReadLine(), out dateOfBirth))
            {
                System.Console.WriteLine("Incorrect input");
            }
            else
            {
                try
                {
                    if (usersBLL.AddUser(new UserVM(firstname, lastname, nickname, email, dateOfBirth,123)))
                    {
                        System.Console.WriteLine("Successfully added");
                    }
                    else
                    {
                        System.Console.WriteLine("Such user already exists");
                    }
                }
                catch (ArgumentException)
                {
                    System.Console.WriteLine("Incorrect date of birth");
                }
            }
            System.Console.ReadLine();
        }

        private static void AddImageToUser()
        {
            System.Console.Write("Enter image's description: ");
            string description = System.Console.ReadLine();


            if (string.IsNullOrWhiteSpace(description))
            {
                System.Console.WriteLine("Incorrect input");
            }
            else
            {
                if (imagesBLL.AddImage(new ImageVM(description)))
                {
                    System.Console.WriteLine("Successfully added");
                }
                else
                {
                    System.Console.WriteLine("Such award already exists");
                }
            }

            var users = usersBLL.GetAllUsers().OrderBy(user => user.LastName);
            int numberUser = GetSelectedNumberOfUser(users);
            if (numberUser != 0)
            {
                var user = users.Skip(numberUser - 1).First();
                var images = imagesBLL.GetAllImages().OrderBy(image => image.DateOfCreating);
                int numberImage = GetSelectedNumberOfImage(images);
                if (numberImage != 0)
                {
                    var image = images.Skip(numberImage - 1).First();
                    if (relationsBLL.AddImageToUser(user.Id, image.Id))
                    {
                        System.Console.WriteLine("Image successfully added to user");
                    }
                    else
                    {
                        System.Console.WriteLine("User already have this award");
                    }
                    System.Console.ReadLine();
                }
            }
        }
        private static void DeleteImageFromUser()
        {
            var users = usersBLL.GetAllUsers().OrderBy(user => user.LastName);
            int numberUser = GetSelectedNumberOfUser(users);
            if (numberUser != 0)
            {
                var user = users.Skip(numberUser - 1).First();
                var images = relationsBLL.GetImagesByUser(user.Id).OrderBy(image => image.DateOfCreating);
                int numberAward = GetSelectedNumberOfImage(images);
                if (numberAward != 0)
                {
                    var award = images.Skip(numberAward - 1).First();
                    relationsBLL.RemoveImageFromUser(user.Id, award.Id);
                    System.Console.WriteLine("Successfully deleted");
                    System.Console.ReadLine();
                }
            }
        }
        private static void DeleteUser()
        {
            var users = usersBLL.GetAllUsers().OrderBy(user => user.LastName);
            int number = GetSelectedNumberOfUser(users);
            if (number != 0)
            {
                var user = users.Skip(number - 1).First();
                usersBLL.RemoveUserById(user.Id);
                System.Console.WriteLine("Successfully deleted");
                System.Console.ReadLine();
            }
        }

        private static void UpdateImage()
        {
            var awards = imagesBLL.GetAllImages().OrderBy(image => image.DateOfCreating);
            int number = GetSelectedNumberOfImage(awards);
            if (number != 0)
            {
                var image = awards.Skip(number - 1).First();
                System.Console.Write("Enter image's description: ");
                string description = System.Console.ReadLine();
                if (string.IsNullOrWhiteSpace(description))
                {
                    System.Console.WriteLine("Incorrect input");
                }
                else
                {
                    if (imagesBLL.UpdateImage(new ImageVM(image.Id, description, image.DateOfCreating)))
                    {
                        System.Console.WriteLine("Successfully updated");
                    }
                    else
                    {
                        System.Console.WriteLine("Such award already exists");
                    }
                }
                System.Console.ReadLine();
            }
        }

        private static void UpdateUser()// изменять можно только имя и дату рождения чтобы не нагромаждать тестовое приложение
        {
            var users = usersBLL.GetAllUsers().OrderBy(user => user.LastName);
            int number = GetSelectedNumberOfUser(users);
            if (number != 0)
            {
                var user = users.Skip(number - 1).First();
                System.Console.Write("Enter user's First Name: ");
                string name = System.Console.ReadLine();

                System.Console.Write("Enter user's date of birth (dd/mm/yyyy): ");
                DateTime dateOfBirth;
                if (string.IsNullOrWhiteSpace(name) | !DateTime.TryParse(System.Console.ReadLine(), out dateOfBirth))
                {
                    System.Console.WriteLine("Incorrect input");
                }
                else
                {
                    try
                    {
                        if (usersBLL.UpdateUser(new UserVM(user.Id, name, user.LastName, user.Nickname, user.Email, dateOfBirth,123)))
                        {
                            System.Console.WriteLine("Successfully updated");
                        }
                        else
                        {
                            System.Console.WriteLine("Such user already exists");
                        }
                    }
                    catch (ArgumentException)
                    {
                        System.Console.WriteLine("Incorrect date of birth");
                    }
                }
                System.Console.ReadLine();
            }
        }

    }
}

