using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using SocketGameProtocol;
using SocketMultplayerGameServer.Servers;

namespace SocketMultplayerGameServer.DAO
{
    class UserData
    {
        /// <summary>
        /// 用户注册数据
        /// </summary>
        /// <param name="pack"></param>
        /// <param name="mySqlConnection"></param>
        /// <returns></returns>
        public bool Logon(Mainpack pack,MySqlConnection mySqlConnection)
        {
            string username = pack.Loginpack.Username;
            string password = pack.Loginpack.Password;
            string playername = pack.Playerpack[0].Playername;

            try
            {
                string sql = "SELECT * FROM userdata WHERE username='" + username + "'";
                MySqlCommand comd = new MySqlCommand(sql, mySqlConnection);
                MySqlDataReader read = comd.ExecuteReader();

                if (read.HasRows)
                {
                    read.Close();
                    return false;
                }
                read.Close();
                sql = "INSERT INTO `sys`.`userdata`(`username`,`password`,`playername`)VALUES('" + username + "','" + password + "','" + playername + "')";
                //插入数据
                comd = new MySqlCommand(sql, mySqlConnection);

                comd.ExecuteNonQuery();
                
                return true;
            }
            catch (Exception e)
            {

                Helper.Log("注册数据库失败"+e.Message);
                return false;
            }
            ;
        }
        /// <summary>
        /// 用户登录数据
        /// </summary>
        /// <param name="pack"></param>
        /// <param name="mySqlConnection"></param>
        /// <returns></returns>
        public bool Login(Mainpack pack, MySqlConnection mySqlConnection)
        {
            string username = pack.Loginpack.Username;
            string password = pack.Loginpack.Password;

            try
            {
                string sql = "SELECT * FROM userdata WHERE username='" + username + "'AND password='" + password + "'";
                MySqlCommand cmd = new MySqlCommand(sql, mySqlConnection);
                MySqlDataReader read = cmd.ExecuteReader();
                
                if (read.HasRows)
                {
                    read.Close();
                    return true;
                }
                else
                {
                    read.Close();
                    return false;
                }
                
            }
            catch (Exception e)
            {
                Helper.Log("链接失败"+e);
                return false;
            }

        }
    }
}
