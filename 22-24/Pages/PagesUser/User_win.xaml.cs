using _22_24.Classes;
using System;
using System.Windows;
using System.Windows.Controls;
using static _22_24.Classes.Common;



namespace _22_24.Pages.PagesUser
{
    /// <summary>
    /// Логика взаимодействия для User_win.xaml
    /// </summary>
    public partial class User_win : Page
    {
        User user_loc;
        public User_win(User _user)
        {
            InitializeComponent();

            user_loc = _user;

            if (_user.fio_user != null )
            {
                fio_user.Text = _user.fio_user;
                phone_user.Text = _user.phone_num;
                addrec_user.Text = _user.pasport_data;
            }
        }

        private void Click_User_Redact(object sender, RoutedEventArgs e)
        {
            try
            {
                Common common = new Common();

                // 1. Проверка полей
                string fio = fio_user.Text.Trim();
                string phone = phone_user.Text.Trim();
                string pasport = addrec_user.Text.Trim();

                MessageBox.Show($"Введенные данные:\nФИО: '{fio}'\nТелефон: '{phone}'\nПаспорт: '{pasport}'");

                if (!common.IsFio(fio))
                {
                    MessageBox.Show("Вы неправильно написали ФИО");
                    return;
                }

                if (!common.IsNumber(phone))
                {
                    MessageBox.Show("Вы неправильно написали номер телефона");
                    return;
                }

                if (string.IsNullOrWhiteSpace(pasport))
                {
                    MessageBox.Show("Введите паспортные данные");
                    return;
                }

                if (user_loc.fio_user == null) // Новый клиент
                {
                    // 2. Пробуем БЕЗ ID
                    string query = $"INSERT INTO [users] ([phone_num], [FIO_user], [pasport_data]) " +
                                  $"VALUES ('{phone}', '{fio}', '{pasport}')";

                    MessageBox.Show("Пробуем запрос 1 (без ID):\n" + query);

                    bool success = MainWindow.connect.ExecuteNonQuery(query);

                    if (success)
                    {
                        MessageBox.Show("Успех с запросом 1!");
                        MainWindow.connect.LoadData(Connection.tabels.users);
                        MainWindow.main.Anim_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.users);
                        return;
                    }

                    // 3. Если не сработало, пробуем С ID
                    int id = MainWindow.connect.SetLastId(Connection.tabels.users);
                    query = $"INSERT INTO [users] ([Код], [phone_num], [FIO_user], [pasport_data]) " +
                           $"VALUES ({id}, '{phone}', '{fio}', '{pasport}')";

                    MessageBox.Show("Пробуем запрос 2 (с ID):\n" + query);

                    success = MainWindow.connect.ExecuteNonQuery(query);

                    if (success)
                    {
                        MessageBox.Show("Успех с запросом 2!");
                        MainWindow.connect.LoadData(Connection.tabels.users);
                        MainWindow.main.Anim_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.users);
                    }
                    else
                    {
                        MessageBox.Show("Оба запроса не сработали");
                    }
                }
                else // Редактирование
                {
                    string query = $"UPDATE [users] SET [phone_num] = '{phone}', " +
                                  $"[FIO_user] = '{fio}', " +
                                  $"[pasport_data] = '{pasport}' " +
                                  $"WHERE [Код] = {user_loc.id}";

                    MessageBox.Show("Запрос на обновление:\n" + query);

                    bool success = MainWindow.connect.ExecuteNonQuery(query);

                    if (success)
                    {
                        MessageBox.Show("Успешное изменение клиента");
                        MainWindow.connect.LoadData(Connection.tabels.users);
                        MainWindow.main.Anim_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.users);
                    }
                    else
                    {
                        MessageBox.Show("Запрос на изменение клиента не сработал");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ОШИБКА: " + ex.Message);
            }
        }
        //private void Click_User_Redact(object sender, RoutedEventArgs e)
        //{
        //    Common common = new Common();
        //    if (!common.IsFio(fio_user.Text))
        //    {
        //        MessageBox.Show("Вы неправильно написали ФИО");
        //        return;
        //    }
        //    if (!common.IsNumber(phone_user.Text))
        //    {
        //        MessageBox.Show("Вы неправильно написали номер телефона");
        //        return;
        //    }
        //    if (addrec_user.Text.Trim() == "")
        //    {
        //        MessageBox.Show("Вы неправильно написали номер паспорта");
        //        return;
        //    }

        //    if (user_loc.fio_user == null)
        //    {
        //        int id = MainWindow.connect.SetLastId(Connection.tabels.users);

        //        string query = $"INSERT INTO [users]([Код], [phone_num], [FIO_user], [pasport_data]) VALUES (" +
        //    $"{id}, " +                    // id - число, без кавычек
        //    $"'{phone_user.Text}', " +      // текст - в кавычках
        //    $"'{fio_user.Text}', " +         // текст - в кавычках
        //    $"'{addrec_user.Text}')";        // текст - в кавычках


        //        var pc = MainWindow.connect.QueryAccess(query);
        //        if (pc != null)
        //        {
        //            MainWindow.connect.LoadData(Connection.tabels.users);
        //            MessageBox.Show("Успешное добавление клиента", "Успешное", MessageBoxButton.OK, MessageBoxImage.Information);
        //            MainWindow.main.Anim_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.users);
        //        }
        //        else
        //        {
        //            MessageBox.Show("Запрос на добавление клиента не был обработан", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        //        }
        //    }
        //    else
        //    {
        //        string query = $"UPDATE [users] SET [phone_num] = '{phone_user.Text}', " +
        //            $"[FIO_user]='{fio_user.Text}', " +
        //            $"[pasport_data]='{addrec_user.Text}' WHERE Код = {user_loc.id}";

        //        var pc = MainWindow.connect.QueryAccess(query);
        //        if (pc != null)
        //        {
        //            MainWindow.connect.LoadData(Connection.tabels.users);
        //            MessageBox.Show("Успешное изменение клиента", "Успешное", MessageBoxButton.OK, MessageBoxImage.Information);
        //            MainWindow.main.Anim_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.users);
        //        }
        //        else
        //        {
        //            MessageBox.Show("Запрос на изменение клиента не был обработан", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        //        }
        //    }
        //}


        private void Click_Cancel_User_Redact(object sender, RoutedEventArgs e)
        {
            MainWindow.main.Anim_move(MainWindow.main.frame_main, MainWindow.main.scroll_main);
        }

        private void Click_Remove_User_Redact(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.connect.LoadData(Connection.tabels.users);
                Call userFind = MainWindow.connect.calls.Find(x => x.user_id == user_loc.id);
                if (userFind != null)
                {
                    var click = MessageBox.Show("У данного клиента есть звонки, всё равно удалить его?", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (click == MessageBoxResult.No)
                    {
                        return;
                    }
                }
                string vs1 = $"DELETE FROM [calls] WHERE [user_id] = '{user_loc.id.ToString()}'";
                var pc1 = MainWindow.connect.QueryAccess(vs1);

                string vs = $"DELETE FROM [users] WHERE [Код] = '{user_loc.id.ToString()}'";

                var pc = MainWindow.connect.QueryAccess(vs);

                if(pc != null && pc1 != null)
                {
                    MessageBox.Show("Успешное удаление аккаунта", "Успешное", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainWindow.connect.LoadData(Connection.tabels.users);
                    MainWindow.main.Anim_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.users);
                }
                else
                {
                    MessageBox.Show("Запрос на удаление клиента не был обработан", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
