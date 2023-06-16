using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace Wife_Main
{
	public partial class Main : Form
	{

		#region 这里为全部的公共属性定义，全局可用

		//出征结构体
		struct BattleMap
		{
			public int BattleMapComboBox1;
			public int BattleMapComboBox2;
			public int BattleMapComboBox3;
			public int BattleMapComboBox4;
			public int BattleMapComboBox5;
			public int BattleMapComboBox6;
			public int BattleMapComboBox8;
			public int BattleMapComboBox11;

			public bool Difficulty; //true为困难，false为简单
			public string DifficultyName;
			public int BattleMapCount; //出征次数
			public int ExpeditionTime; //远征时间



		}

		public bool ComBatSwitch; //出征任务情况
		public bool ExpeditionSwitch; //远征任务情况
		public bool WifeExamine; //船只出征检查状态
		public bool WifeHealthStatus; //船只完好确认 过了出征检查后会赋值为true，每次战斗后必须赋值为false
		public int Fully_Invested = 0; //这个属性用于检测满仓，1为满 0为不满
		public bool Combat_Condition; //这个属性进入战斗后为true，结束为false 用于战斗中夜战判断
		public int Number_of_Battles; //单次战斗的次数
		public int Number_of_Battles_Sum; //总的战斗次数





		BattleMap battleMap = new BattleMap();

		//出征数组定义
		readonly string[] BattleMap9 = new string[2] { "9-1", "9-2", };
		readonly string[] BattleMap8 = new string[5] { "8-1", "8-2", "8-3", "8-4", "8-5" };
		readonly string[] BattleMap7 = new string[5] { "7-1", "7-2", "7-3", "7-4", "7-5" };
		readonly string[] BattleMap6 = new string[4] { "6-1", "6-2", "6-3", "6-4" };
		readonly string[] BattleMap5 = new string[5] { "5-1", "5-2", "5-3", "5-4", "5-5" };
		readonly string[] BattleMap4 = new string[4] { "4-1", "4-2", "4-3", "4-4" };
		readonly string[] BattleMap3 = new string[4] { "3-1", "3-2", "3-3", "3-4" };
		readonly string[] BattleMap2 = new string[6] { "2-1", "2-2", "2-3", "2-4", "2-5", "2-6" };
		readonly string[] BattleMap1 = new string[5] { "1-1", "1-2", "1-3", "1-4", "1-5" };

		public static IntPtr MainGameProgression; //游戏窗口进程
		public static Image LiveInterface; //游戏窗口画面

		//公共属性
		public int times;//鼠标点击间隔
		public int mouse_er; //鼠标误差
		public static int execute_sum; //执行次数
		public static string Dg = Directory.GetCurrentDirectory() + "\\Data\\"; //配置文件路径
		public static string Mian_info; //全局信息输出，由计时器输出
		public DateTime de = DateTime.Now; //当前时间
		public static string Log = Directory.GetCurrentDirectory() + "\\Wife_Log.txt"; //日志文件路径



		public static DateTime ks, js, hctime, temptimes, starttime;
		public static TimeSpan Run_time; //单次出征时间
		public static double Run_times; //总出征时间
		public static int NumberOfRepairs; //维修次数统计

		
		public int Number_of_battles_Re = 1; //单次战斗的次数 计时器使用参数

		//每个界面信息定义
		public string HomePath = Dg + "Home.wife"; //主页
		public string Go_On_An_Expedition = Dg + "Go_On_An_Expedition.wife"; //主页到出征界面 
		public string Expedition = Dg + "Expedition.wife"; //主页到远征界面
		public string Formation = Dg + "Formation.wife"; //编队界面
		public string Battle_Array = Dg + "Battle_Array.wife"; //索敌完成界面，准备打或者撤退
		public string Our_Formation = Dg + "Our_Formation.wife"; //我方阵型选择界面
		public string Result = Dg + "Result.wife"; //战斗结果界面
		public string Shipping = Dg + "Shipping.wife"; //船只界面，出货界面
		public string Forward_And_Backward = Dg + "Forward_And_Backward.wife"; //战斗结束后 无大破的前进与后退
		public string Post_War_Disadvantage = Dg + "Post_War_Disadvantage.wife"; //战斗结束后 有大破的前进与后退






		public string Night_Fighting_Or_Battle = Dg + "\\Night_Fighting_Or_Battle\\"; //是否要夜战 界面 1005
		public string Our_Team = Dg + "\\Our_Team\\"; //我方阵容选择界面 1007

		public string Full_Position = Dg + "\\Full_Position\\"; //出征界面 船只满位 1009
		
		public string Flagship_Damage = Dg + "\\Flagship_Damage\\"; //旗舰大破 1011
		public string Disassemble_Wife = Dg + "\\Disassemble_Wife\\Disassemble_Wife\\"; //拆解船只界面 1012
		public string Disassemble_Wife_Add = Dg + "\\Disassemble_Wife\\Disassemble_Wife_Add\\"; //拆解船只界面添加船只 1013
		public string Disassemble_Wife_Add_OK = Dg + "\\Disassemble_Wife\\Disassemble_Wife_Add_OK\\"; //拆解船只界面添加船只完成 1014
		public string Disassemble_Wife_Add_OK_Mes = Dg + "\\Disassemble_Wife\\Disassemble_Wife_Add_OK_Mes\\"; //拆解船只界面添加船只完成且含有四星船弹窗 1015
		public string HomePathBroadside = Dg + "\\HomePathBroadside\\"; //主页侧边栏 1016

		//远征相关
		public string Expedition_Resource_01 = Dg + "\\Expedition_Resource_01.wife"; //远征一号位
		public string Expedition_Resource_02 = Dg + "\\Expedition_Resource_02.wife"; //远征二号位
		public string Expedition_Resource_03 = Dg + "\\Expedition_Resource_03.wife"; //远征三号位
		public string Expedition_Resource_04 = Dg + "\\Expedition_Resource_04.wife"; //远征四号位
		public string Expedition_Resource_info = Dg + "\\Expedition_Resource_info.wife"; //红色感叹号 用来判断是否有资源要收集
		public string Expedition_Resource_UI = Dg + "\\Expedition_Resource_UI.wife"; //远征成功界面
		public string Expedition_Resource_END = Dg + "\\Expedition_Resource_END.wife"; //远征完成弹窗

		//老婆耐久识别 色卡文件为红血大破
		public string Wife_Life1 = Dg + "Wife_Life1.wife";
		public string Wife_Life2 = Dg + "Wife_Life2.wife";
		public string Wife_Life3 = Dg + "Wife_Life3.wife";
		public string Wife_Life4 = Dg + "Wife_Life4.wife";
		public string Wife_Life5 = Dg + "Wife_Life5.wife";
		public string Wife_Life6 = Dg + "Wife_Life6.wife";









		#endregion

		public Main()
		{
			InitializeComponent();
		}

		/// <summary>
		/// 用户点位传递,返回对应的点位置
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Point User_Point(int value)
		{
			Point user = new Point();
			switch (value)
			{
				case 0: { user.X = 1270; user.Y = 360; }; break;//跳过键
				case 1: { user.X = 1200; user.Y = 660; }; break;//主页中出征
				case 2: { user.X = 230; user.Y = 30; }; break;//出征页面中的出征
				case 3: { user.X = 560; user.Y = 30; }; break;//出征页面中的远征
				case 4: { user.X = 800; user.Y = 400; }; break;//出征页面中的大地图
				case 5: { user.X = 1100; user.Y = 670; }; break;//出征页面中的 开始出征
				case 6: { user.X = 1150; user.Y = 660; }; break;//索敌完成中的 开始战斗
				case 7: { user.X = 940; user.Y = 660; }; break;//索敌完成中的 撤退
				case 8: { user.X = 440; user.Y = 460; }; break;//大地图中 无大破的前进与后退界面中的前进
				case 9: { user.X = 840; user.Y = 460; }; break;//大地图中 无大破的前进与后退界面中的回港

				case 99: { user.X = 40; user.Y = 40; }; break;//左上角返回键

				case 31: { user.X = 1150; user.Y = 195; }; break;//出征页面中的远征资源1
				case 32: { user.X = 1150; user.Y = 330; }; break;//出征页面中的远征资源2
				case 33: { user.X = 1150; user.Y = 480; }; break;//出征页面中的远征资源3
				case 34: { user.X = 1150; user.Y = 620; }; break;//出征页面中的远征资源4
				case 35: { user.X = 480; user.Y = 450; }; break;//出征页面中的远征资源 的 再次出征

				case 60: { user.X = 550; user.Y = 560; }; break;//编队点击维修
				case 61: { user.X = 130; user.Y = 300; }; break;//编队维修老婆1
				case 62: { user.X = 280; user.Y = 300; }; break;//编队维修老婆2
				case 63: { user.X = 420; user.Y = 300; }; break;//编队维修老婆3
				case 64: { user.X = 580; user.Y = 300; }; break;//编队维修老婆4
				case 65: { user.X = 720; user.Y = 300; }; break;//编队维修老婆5
				case 66: { user.X = 870; user.Y = 300; }; break;//编队维修老婆6

				case 71: { user.X = 870; user.Y = 300; }; break;//单纵
				case 72: { user.X = 870; user.Y = 300; }; break;//复纵
				case 73: { user.X = 870; user.Y = 300; }; break;//轮形
				case 74: { user.X = 870; user.Y = 300; }; break;//梯形
				case 75: { user.X = 870; user.Y = 300; }; break;//单横
			}

			return user;
		}


		/// <summary>
		/// 窗体加载程序
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_Load(object sender, EventArgs e)
		{
			comboBox7.SelectedIndex = 0;

			//出征
			comboBox1.SelectedIndex = Properties.Settings.Default.comboBox1;
			comboBox2.SelectedIndex = Properties.Settings.Default.comboBox2;
			comboBox3.SelectedIndex = Properties.Settings.Default.comboBox3;
			comboBox4.SelectedIndex = Properties.Settings.Default.comboBox4;
			textBox1.Text = Properties.Settings.Default.textBox1sum.ToString();
			comboBox10.SelectedIndex = Properties.Settings.Default.comboBox10;

			//战役
			comboBox5.SelectedIndex = Properties.Settings.Default.comboBox5;
			comboBox6.SelectedIndex = Properties.Settings.Default.comboBox6;

			//演习
			comboBox8.SelectedIndex = Properties.Settings.Default.comboBox8;
			comboBox11.SelectedIndex = Properties.Settings.Default.comboBox11;

			
			textBox2.Text = Properties.Settings.Default.textBox2time.ToString();//远征时间
			checkBox11.Checked = Properties.Settings.Default.checkBox11; //是否夜战
		}

		/// <summary>
		/// 这个计时器用于监听屏幕与线程
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timer1_Tick(object sender, EventArgs e)
		{
			MainGameProgression = Wife_Core.GetSubform(Wife_Core.FindWindow(null, comboBox7.Text));
			LiveInterface = Wife_Core.CaptureWindow(MainGameProgression);
			pictureBox1.BackgroundImage = Wife_Core.CaptureWindow(MainGameProgression);
			//listBox1.Items.Add("窗口大小为" + pictureBox1.BackgroundImage.Size);
			pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;

			if (listBox1.Items.Count > 100)
			{
				listBox1.Items.Clear();
			}
			listBox1.SelectedIndex = listBox1.Items.Count - 1;
			GC.Collect();
		}

		/// <summary>
		/// 出征地图
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.comboBox1 = comboBox1.SelectedIndex;
			Properties.Settings.Default.Save();

			comboBox2.Items.Clear();
			switch (comboBox1.SelectedIndex)
			{
				case 8: { comboBox2.Items.AddRange(BattleMap1); comboBox2.SelectedIndex = 0; }; break;
				case 7: { comboBox2.Items.AddRange(BattleMap2); comboBox2.SelectedIndex = 0; }; break;
				case 6: { comboBox2.Items.AddRange(BattleMap3); comboBox2.SelectedIndex = 0; }; break;
				case 5: { comboBox2.Items.AddRange(BattleMap4); comboBox2.SelectedIndex = 0; }; break;
				case 4: { comboBox2.Items.AddRange(BattleMap5); comboBox2.SelectedIndex = 0; }; break;
				case 3: { comboBox2.Items.AddRange(BattleMap6); comboBox2.SelectedIndex = 0; }; break;
				case 2: { comboBox2.Items.AddRange(BattleMap7); comboBox2.SelectedIndex = 0; }; break;
				case 1: { comboBox2.Items.AddRange(BattleMap8); comboBox2.SelectedIndex = 0; }; break;
				case 0: { comboBox2.Items.AddRange(BattleMap9); comboBox2.SelectedIndex = 0; }; break;
				default: break;
			}
		}

		/// <summary>
		/// 出征地图 详细分支
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.comboBox2 = comboBox2.SelectedIndex;
			Properties.Settings.Default.Save();
			listBox1.Items.Add(comboBox2.SelectedIndex);
		}

		/// <summary>
		/// 出征阵容
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.comboBox3 = comboBox3.SelectedIndex;
			Properties.Settings.Default.Save();
		}

		/// <summary>
		/// 出征阵容，队形
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.comboBox4 = comboBox4.SelectedIndex;
			Properties.Settings.Default.Save();
		}

		/// <summary>
		/// 战役 地图选择
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.comboBox6 = comboBox6.SelectedIndex;
			Properties.Settings.Default.Save();
		}

		/// <summary>
		/// 战役 出征阵容
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.comboBox5 = comboBox5.SelectedIndex;
			Properties.Settings.Default.Save();
		}

		/// <summary>
		/// 添加出征任务
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button1_Click(object sender, EventArgs e)
		{
			battleMap.BattleMapComboBox1 = comboBox1.SelectedIndex;
			battleMap.BattleMapComboBox2 = comboBox2.SelectedIndex;
			battleMap.BattleMapComboBox3 = comboBox3.SelectedIndex;
			battleMap.BattleMapComboBox4 = comboBox4.SelectedIndex;
			battleMap.BattleMapCount = int.Parse(textBox1.Text);
			ComBatSwitch = true;
			listBox3.Items.Add("【出征】" + comboBox1.Text + "\\" + comboBox2.Text + "\\" + comboBox3.Text + "\\" + comboBox4.Text + "\\次数" + textBox1.Text);
			listBox1.Items.Add("添加出征任务成功");
		}

		/// <summary>
		/// 演习队伍选择
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.comboBox8 = comboBox8.SelectedIndex;
			Properties.Settings.Default.Save();
		}

		/// <summary>
		/// 演习阵容
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboBox11_SelectedIndexChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.comboBox11 = comboBox11.SelectedIndex;
			Properties.Settings.Default.Save();
		}

		/// <summary>
		/// 添加演习任务
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button7_Click(object sender, EventArgs e)
		{
			battleMap.BattleMapComboBox8 = comboBox8.SelectedIndex;
			battleMap.BattleMapComboBox11 = comboBox11.SelectedIndex;
			listBox3.Items.Add("【演习】" + comboBox8.Text + "\\" + comboBox11.Text);
			listBox1.Items.Add("添加演习任务成功");
		}

		/// <summary>
		/// 出征次数
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			if (textBox1.Text != "")
			{
				Properties.Settings.Default.textBox1sum = int.Parse(textBox1.Text);
				Properties.Settings.Default.Save();
			}
			else
			{
				MessageBox.Show("出征次数不可以为空");
				textBox1.Text = Properties.Settings.Default.textBox1sum.ToString();
			}
		}

		/// <summary>
		/// 出征次数判断 禁止输入数字以外的字符
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
		{
			// 判断输入的字符是否为数字或退格键
			if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
			{
				// 如果不是数字或退格键，则禁止输入
				e.Handled = true;
			}
		}

		/// <summary>
		/// 添加远征任务
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button2_Click(object sender, EventArgs e)
		{
			battleMap.ExpeditionTime = int.Parse(textBox2.Text) * 60000;//将分钟换算为毫秒
			ExpeditionSwitch = true; //开始远征
			listBox3.Items.Add("【远征】" + "每隔" + textBox2.Text + "分钟将检查远征情况");
			listBox1.Items.Add("添加远征任务成功");
		}

		/// <summary>
		/// 远征分钟频率
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
		{
			// 判断输入的字符是否为数字或退格键
			if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
			{
				// 如果不是数字或退格键，则禁止输入
				e.Handled = true;
			}
		}

		/// <summary>
		/// 远征时间输入
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textBox2_TextChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.textBox2time = int.Parse(textBox2.Text);
			Properties.Settings.Default.Save();
		}

		/// <summary>
		/// 战斗次数
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboBox10_SelectedIndexChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.comboBox10 = comboBox10.SelectedIndex;
			Properties.Settings.Default.Save();
		}

		/// <summary>
		/// 根据当前任务列表，结合timer1得到的界面信息，执行对应操作
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timer2_Tick(object sender, EventArgs e)
		{

		}

		/// <summary>
		/// 添加战役任务
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button3_Click(object sender, EventArgs e)
		{
			battleMap.BattleMapComboBox5 = comboBox5.SelectedIndex;
			battleMap.BattleMapComboBox6 = comboBox6.SelectedIndex;
			if (radioButton3.Checked == true)
			{
				battleMap.Difficulty = true;
				battleMap.DifficultyName = "困难";
			}
			else
			{
				battleMap.Difficulty = false;
				battleMap.DifficultyName = "简单";
			}
			listBox3.Items.Add("【战役】" + comboBox6.Text + "\\" + comboBox5.Text + "\\" + battleMap.DifficultyName);
			listBox1.Items.Add("添加战役任务成功");
		}

		/// <summary>
		/// 护肝结束
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button11_Click(object sender, EventArgs e)
		{
			ExpeditionSwitch = false;
			ComBatSwitch = false;
		}

		/// <summary>
		/// 护肝开始
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button6_Click(object sender, EventArgs e)
		{
			//任务列表开始
			if (ExpeditionSwitch == true)
			{
				ExpeditionMain();
			}

			if (ComBatSwitch == true)
			{
				ComBatMain();
			}
		}

		/// <summary>
		/// 调试
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button9_Click(object sender, EventArgs e)
		{
			if (Wife_Core.Home(Post_War_Disadvantage) == true)
			{
				MessageBox.Show("111");
			}

		}

		/// <summary>
		/// 是否夜战
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void checkBox11_CheckedChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.checkBox11 = checkBox11.Checked;
			Properties.Settings.Default.Save();
		}

		/// <summary>
		/// 调试
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button10_Click(object sender, EventArgs e)
		{

		}

		#region 远征主函数的逻辑

		/// <summary>
		/// 远征主函数
		/// </summary>
		private async void ExpeditionMain()
		{
			while (ExpeditionSwitch == true)
			{
				await Task.Run(() => { Thread.Sleep(3000); });

				//判断当前是否在主页
				if (Wife_Core.Home(HomePath) == true)
				{
					listBox1.Items.Add(DateTime.Now + "远征 - 当前位于主页");
					//点击出征
					Wife_Core.Clicks(MainGameProgression, User_Point(1));
					await Task.Run(() => { Thread.Sleep(3000); });
				}

				//判断当前是否在出征页面
				if (Wife_Core.Home(Go_On_An_Expedition) == true)
				{
					listBox1.Items.Add(DateTime.Now + "远征 - 当前位于出征");
					//这里进来不是远征的话，默认回到主页去去算了
					Wife_Core.Clicks(MainGameProgression, User_Point(99));
					//等待一段时间后再点开出征
					Console.WriteLine(battleMap.ExpeditionTime);
					await Task.Run(() => { Thread.Sleep(battleMap.ExpeditionTime); });
				}

				//判断当前是否在远征界面
				if (Wife_Core.Home(Expedition) == true)
				{
					listBox1.Items.Add(DateTime.Now + "远征 - 当前位于远征");
					//判断当前是否有资源可以收集
					if (Wife_Core.Home(Expedition_Resource_info) == true)
					{
						//判断第一行
						if (Wife_Core.Home(Expedition_Resource_01) == true)
						{
							Wife_Core.Clicks(MainGameProgression, User_Point(31));
							listBox2.Items.Add(DateTime.Now + "收集了第一行的资源");
							await Task.Run(() => { Thread.Sleep(3000); });
						}

						//判断第二行
						if (Wife_Core.Home(Expedition_Resource_02) == true)
						{
							Wife_Core.Clicks(MainGameProgression, User_Point(32));
							listBox2.Items.Add(DateTime.Now + "收集了第二行的资源");
							await Task.Run(() => { Thread.Sleep(3000); });
						}

						//判断第三行
						if (Wife_Core.Home(Expedition_Resource_03) == true)
						{
							Wife_Core.Clicks(MainGameProgression, User_Point(33));
							listBox2.Items.Add(DateTime.Now + "收集了第三行的资源");
							await Task.Run(() => { Thread.Sleep(3000); });
						}

						//判断第四行
						if (Wife_Core.Home(Expedition_Resource_04) == true)
						{
							Wife_Core.Clicks(MainGameProgression, User_Point(34));
							listBox2.Items.Add(DateTime.Now + "收集了第四行的资源");
							await Task.Run(() => { Thread.Sleep(3000); });
						}
					}
					else
					{
						listBox1.Items.Add(DateTime.Now + "返回主页");
						//左上角返回主页
						Wife_Core.Clicks(MainGameProgression, User_Point(99));
						await Task.Run(() => { Thread.Sleep(3000); });
					}
				}

				//判断当前界面是否是远征完成界面
				if (Wife_Core.Home(Expedition_Resource_UI) == true)
				{
					listBox1.Items.Add(DateTime.Now + "远征 - 当前位于远征成功界面");
					Wife_Core.CreateImage(3);
					await Task.Run(() => { Thread.Sleep(3000); });
					//点击下一步
					Wife_Core.Clicks(MainGameProgression, User_Point(0));
					await Task.Run(() => { Thread.Sleep(3000); });
				}

				//判断当前界面是否是远征完成界面 后 的弹窗
				if (Wife_Core.Home(Expedition_Resource_END) == true)
				{
					listBox1.Items.Add(DateTime.Now + "远征 - 当前位于远征成功界面");
					//点击再次出征
					Wife_Core.Clicks(MainGameProgression, User_Point(35));
					await Task.Run(() => { Thread.Sleep(3000); });
				}

				//while函数最后加一个暂停，不然一直高频
				await Task.Run(() => { Thread.Sleep(3000); });
			}
		}

		#endregion

		#region 出征函数的相关逻辑

		/// <summary>
		/// 出征主函数
		/// </summary>
		private async void ComBatMain()
		{
			while (ComBatSwitch == true)
			{
				await Task.Run(() => { Thread.Sleep(3000); });

				//判断当前是否在主页
				if (Wife_Core.Home(HomePath) == true)
				{
					listBox1.Items.Add(DateTime.Now + "出征 - 当前位于主页");
					//点击出征
					Wife_Core.Clicks(MainGameProgression, User_Point(1));
					await Task.Run(() => { Thread.Sleep(3000); });
				}

				//判断当前是否在出征页面
				if (Wife_Core.Home(Go_On_An_Expedition) == true)
				{
					//退出出征函数 如果计数满了的话
					if (Number_of_Battles_Sum == int.Parse(textBox1.Text))
					{
						listBox1.Items.Add(DateTime.Now + "出征 - 任务完成");
						break;
					}

					listBox1.Items.Add(DateTime.Now + "出征 - 当前位于出征");
					//点击大地图
					Wife_Core.Clicks(MainGameProgression, User_Point(4));
					await Task.Run(() => { Thread.Sleep(3000); });

				}

				//判断当前是否在远征界面
				if (Wife_Core.Home(Expedition) == true)
				{
					listBox1.Items.Add(DateTime.Now + "出征 - 当前位于远征");
					//点击出征
					Wife_Core.Clicks(MainGameProgression, User_Point(2));
					await Task.Run(() => { Thread.Sleep(3000); });
				}

				//判断当前是否在编队
				if (Wife_Core.Home(Formation) == true)
				{
					listBox1.Items.Add(DateTime.Now + "出征 - 当前位于编队");

					if (WifeExamine == false)
					{
						Maintenance(); //过一遍船只检查
					}

					//点击 开始出征
					if (WifeHealthStatus == true)
					{
						//点击 开始出征
						listBox1.Items.Add(DateTime.Now + "出征 - 开始出征");
						Wife_Core.Clicks(MainGameProgression, User_Point(5));
						await Task.Run(() => { Thread.Sleep(3000); });
					}
					await Task.Run(() => { Thread.Sleep(3000); });
				}

				//判断当前是否是索敌完成
				if (Wife_Core.Home(Battle_Array) == true)
				{
					listBox1.Items.Add(DateTime.Now + "出征 - 索敌完成");
					Wife_Core.Clicks(MainGameProgression, User_Point(6));
					await Task.Run(() => { Thread.Sleep(3000); });
				}

				//判断当前是否我方选择 阵型
				if (Wife_Core.Home(Our_Formation) == true)
				{
					switch (comboBox4.SelectedIndex)
					{
						case 0: { Wife_Core.Clicks(MainGameProgression, User_Point(71)); listBox1.Items.Add(DateTime.Now + "出征 - 单纵"); }; break;//单纵
						case 1: { Wife_Core.Clicks(MainGameProgression, User_Point(72)); listBox1.Items.Add(DateTime.Now + "出征 - 复纵"); }; break;//复纵
						case 2: { Wife_Core.Clicks(MainGameProgression, User_Point(73)); listBox1.Items.Add(DateTime.Now + "出征 - 轮形"); }; break;//轮形
						case 3: { Wife_Core.Clicks(MainGameProgression, User_Point(74)); listBox1.Items.Add(DateTime.Now + "出征 - 梯形"); }; break;//梯形
						case 4: { Wife_Core.Clicks(MainGameProgression, User_Point(75)); listBox1.Items.Add(DateTime.Now + "出征 - 单横"); }; break;//单横
					}
					Combat_Condition = true; //进入战斗
					Number_of_Battles_Sum++; //总的战斗次数
					Number_of_Battles++; //单次战斗次数
					listBox1.Items.Add(DateTime.Now + "出征 - 当前进行第" + Number_of_Battles_Sum + "次战斗");
					listBox1.Items.Add(DateTime.Now + "出征 - 还剩" + (int.Parse(textBox1.Text) - Number_of_Battles_Sum) + "次战斗");
					await Task.Run(() => { Thread.Sleep(3000); });
				}

				//判断当前是否战斗结束
				if (Wife_Core.Home(Result) == true)
				{
					listBox1.Items.Add(DateTime.Now + "出征 - 战斗结束");

					WifeHealthStatus = false; //船只检查状态未知
					Combat_Condition = false; //非战斗状态

					Wife_Core.Clicks(MainGameProgression, User_Point(0));
					await Task.Run(() => { Thread.Sleep(3000); });
				}

				//判断当前是否 掉落船只
				if (Wife_Core.Home(Shipping) == true)
				{
					listBox1.Items.Add(DateTime.Now + "出征 - 打捞到船只了");
					Wife_Core.CreateImage(1); //截图
					await Task.Run(() => { Thread.Sleep(3000); });
					Wife_Core.Clicks(MainGameProgression, User_Point(0));
					await Task.Run(() => { Thread.Sleep(3000); });
				}

				//判断当前是否是 无大破的前进与后退 需要满足不在战斗状态且非大破
				if ((Wife_Core.Home(Forward_And_Backward) == true) && (Combat_Condition == false))
				{
					listBox1.Items.Add(DateTime.Now + "出征 - 判断前进与后退");

					if (Number_of_Battles < int.Parse(comboBox10.Text))
					{
						listBox1.Items.Add(DateTime.Now + "出征 - 前进");
						Wife_Core.Clicks(MainGameProgression, User_Point(8));
					}
					else
					{
						listBox1.Items.Add(DateTime.Now + "出征 - 回港");
						Wife_Core.Clicks(MainGameProgression, User_Point(9));
					}

					await Task.Run(() => { Thread.Sleep(3000); });
				}

				//判断当前是否是 夜战中追击与放弃 需要满足在战斗状态
				if ((Wife_Core.Home(Forward_And_Backward) == true) && (Combat_Condition == true))
				{
					listBox1.Items.Add(DateTime.Now + "出征 - 判断是否夜战");

					if (checkBox11.Checked == true)
					{
						listBox1.Items.Add(DateTime.Now + "出征 - 夜战进击");
						Wife_Core.Clicks(MainGameProgression, User_Point(8));
					}
					else
					{
						listBox1.Items.Add(DateTime.Now + "出征 - 夜战撤退");
						Wife_Core.Clicks(MainGameProgression, User_Point(9));
					}

					await Task.Run(() => { Thread.Sleep(3000); });
				}




				//判断结束后是否是 有大破船只 这种情况一定回
				if (Wife_Core.Home(Post_War_Disadvantage) == true)
				{
					listBox1.Items.Add(DateTime.Now + "出征 - 战后队伍内有大破 回港");
					Wife_Core.Clicks(MainGameProgression, User_Point(9));
					await Task.Run(() => { Thread.Sleep(3000); });
				}

				await Task.Run(() => { Thread.Sleep(3000); });
			}
		}

		#endregion

		#region 维修检查的相关函数

		/// <summary>
		/// 维修检测
		/// </summary>
		private async void Maintenance()
		{
			WifeExamine = true;

			while (true)
			{
				//点击维修
				Wife_Core.Clicks(MainGameProgression, User_Point(60));
				await Task.Run(() => { Thread.Sleep(3000); });

				//一号位
				if (Wife_Core.Home(Wife_Life1) == true)
				{
					listBox1.Items.Add(DateTime.Now + "一号位老婆大破");
					Wife_Core.CreateImage(2); //截图
					await Task.Run(() => { Thread.Sleep(3000); });
					Wife_Core.Clicks(MainGameProgression, User_Point(61));
					await Task.Run(() => { Thread.Sleep(3000); });
				}

				//二号位
				if (Wife_Core.Home(Wife_Life2) == true)
				{
					listBox1.Items.Add(DateTime.Now + "二号位老婆大破");
					Wife_Core.CreateImage(2); //截图
					await Task.Run(() => { Thread.Sleep(3000); });
					Wife_Core.Clicks(MainGameProgression, User_Point(62));
					await Task.Run(() => { Thread.Sleep(3000); });
				}

				//三号位
				if (Wife_Core.Home(Wife_Life3) == true)
				{
					listBox1.Items.Add(DateTime.Now + "三号位老婆大破");
					Wife_Core.CreateImage(2); //截图
					await Task.Run(() => { Thread.Sleep(3000); });
					Wife_Core.Clicks(MainGameProgression, User_Point(63));
					await Task.Run(() => { Thread.Sleep(3000); });
				}

				//四号位
				if (Wife_Core.Home(Wife_Life4) == true)
				{
					listBox1.Items.Add(DateTime.Now + "四号位老婆大破");
					Wife_Core.CreateImage(2); //截图
					await Task.Run(() => { Thread.Sleep(3000); });
					Wife_Core.Clicks(MainGameProgression, User_Point(64));
					await Task.Run(() => { Thread.Sleep(3000); });
				}

				//五号位
				if (Wife_Core.Home(Wife_Life5) == true)
				{
					listBox1.Items.Add(DateTime.Now + "五号位老婆大破");
					Wife_Core.CreateImage(2); //截图
					await Task.Run(() => { Thread.Sleep(3000); });
					Wife_Core.Clicks(MainGameProgression, User_Point(65));
					await Task.Run(() => { Thread.Sleep(3000); });
				}

				//六号位
				if (Wife_Core.Home(Wife_Life6) == true)
				{
					listBox1.Items.Add(DateTime.Now + "六号位老婆大破");
					Wife_Core.CreateImage(2); //截图
					await Task.Run(() => { Thread.Sleep(3000); });
					Wife_Core.Clicks(MainGameProgression, User_Point(66));
					await Task.Run(() => { Thread.Sleep(3000); });
				}

				if ((Wife_Core.Home(Wife_Life1) == false) &&
					(Wife_Core.Home(Wife_Life2) == false) &&
					(Wife_Core.Home(Wife_Life3) == false) &&
					(Wife_Core.Home(Wife_Life4) == false) &&
					(Wife_Core.Home(Wife_Life5) == false) &&
					(Wife_Core.Home(Wife_Life6) == false))
				{
					//全部老婆不是红血
					listBox1.Items.Add(DateTime.Now + "全舰队无大破");
					WifeExamine = false; //退出检查函数
					WifeHealthStatus = true; //确认船只完好
					break;
				}

				await Task.Run(() => { Thread.Sleep(3000); });
			}
		}

		#endregion




	}
}
