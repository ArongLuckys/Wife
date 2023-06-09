﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace Wife_Main
{
	public partial class Main : Form
	{
		public Main()
		{
			InitializeComponent();
		}

		//出征结构体
		struct BattleMap
		{
			public int BattleMapComboBox1;
			public int BattleMapComboBox2;
			public int BattleMapComboBox3;
			public int BattleMapComboBox4;
		}

		readonly string[] BattleMap9 = new string[2] { "9-1", "9-2", };
		readonly string[] BattleMap8 = new string[5] { "8-1", "8-2", "8-3", "8-4", "8-5" };
		readonly string[] BattleMap7 = new string[5] { "7-1", "7-2", "7-3", "7-4", "7-5" };
		readonly string[] BattleMap6 = new string[4] { "6-1", "6-2", "6-3", "6-4" };
		readonly string[] BattleMap5 = new string[5] { "5-1", "5-2", "5-3", "5-4", "5-5" };
		readonly string[] BattleMap4 = new string[4] { "4-1", "4-2", "4-3", "4-4" };
		readonly string[] BattleMap3 = new string[4] { "3-1", "3-2", "3-3", "3-4" };
		readonly string[] BattleMap2 = new string[6] { "2-1", "2-2", "2-3", "2-4", "2-5", "2-6" };
		readonly string[] BattleMap1 = new string[5] { "1-1", "1-2", "1-3", "1-4", "1-5" };





		IntPtr MainGameProgression; //主游戏窗口进程

		//公共属性
		public int times;//鼠标点击间隔
		public int mouse_er; //鼠标误差
		public static int execute_sum; //执行次数
		Random rd = new Random(); //浮点数
		public static string Dg = Directory.GetCurrentDirectory() + "\\Data"; //配置文件路径
		public Point pt; //全局唯一坐标点
		public static string Mian_info; //全局信息输出，由计时器输出
		public DateTime de = DateTime.Now; //当前时间
		public static string Log = Directory.GetCurrentDirectory() + "\\Wife_Log.txt"; //日志文件路径
		public static int UI = 0; //这个设置窗口显示，如果是1则显示主窗口
		public static int Fully_Invested = 0; //这个属性用于检测满仓，1为满 0为不满
		public static int Combat_Condition = 0; //这个属性进入战斗后为1，结束为0 用于战斗中夜战判断

		public static DateTime ks, js, hctime, temptimes, starttime;
		public static TimeSpan Run_time; //单次出征时间
		public static double Run_times; //总出征时间
		public static int NumberOfRepairs; //维修次数统计

		public int Number_of_battles; //单次战斗的次数
		public int Number_of_battles_Re = 1; //单次战斗的次数 计时器使用参数

		//每个界面信息定义
		public string HomePath = Dg + "\\Home\\"; //主页 1000
		public string Go_On_An_Expedition = Dg + "\\Go_On_An_Expedition\\"; //出征界面 1001
		public string Formation = Dg + "\\Formation\\"; //编队界面 1002
		public string Expedition = Dg + "\\Expedition\\"; //远征界面 1003
		public string Battle_Array = Dg + "\\Battle_Array\\"; //索敌完成界面，准备打或者撤退 1004
		public string Night_Fighting_Or_Battle = Dg + "\\Night_Fighting_Or_Battle\\"; //是否要夜战 界面 1005
		public string Result = Dg + "\\Result\\"; //战斗结果界面 1006
		public string Our_Team = Dg + "\\Our_Team\\"; //我方阵容选择界面 1007
		public string Shipping = Dg + "\\Shipping\\"; //船只界面，出货界面 1008
		public string Full_Position = Dg + "\\Full_Position\\"; //出征界面 船只满位 1009
		public string Post_War_Disadvantage = Dg + "\\Post_War_Disadvantage\\"; //战后有大破船 1010
		public string Flagship_Damage = Dg + "\\Flagship_Damage\\"; //旗舰大破 1011
		public string Disassemble_Wife = Dg + "\\Disassemble_Wife\\Disassemble_Wife\\"; //拆解船只界面 1012
		public string Disassemble_Wife_Add = Dg + "\\Disassemble_Wife\\Disassemble_Wife_Add\\"; //拆解船只界面添加船只 1013
		public string Disassemble_Wife_Add_OK = Dg + "\\Disassemble_Wife\\Disassemble_Wife_Add_OK\\"; //拆解船只界面添加船只完成 1014
		public string Disassemble_Wife_Add_OK_Mes = Dg + "\\Disassemble_Wife\\Disassemble_Wife_Add_OK_Mes\\"; //拆解船只界面添加船只完成且含有四星船弹窗 1015
		public string HomePathBroadside = Dg + "\\HomePathBroadside\\"; //主页侧边栏 1016



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

			//战役
			comboBox5.SelectedIndex = 0;
			comboBox6.SelectedIndex = 0;

			comboBox8.SelectedIndex = 0;
		}

		/// <summary>
		/// 这个计时器用于监听屏幕与线程
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timer1_Tick(object sender, EventArgs e)
		{
			MainGameProgression = Wife_Core.GetSubform(Wife_Core.FindWindow(null, comboBox7.Text));
			pictureBox1.BackgroundImage = Wife_Core.CaptureWindow(MainGameProgression);
			listBox1.Items.Add(pictureBox1.BackgroundImage.Size);
			//pictureBox1.BackgroundImage = Wife_Core.CaptureWindow(Wife_Core.FindWindow(null, comboBox7.Text));
			pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;


			if (Wife_Main.Home(HomePath) == true)
			{
				listBox1.Items.Add("当前位于主页");
			}

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
			BattleMap battleMap = new BattleMap
			{
				BattleMapComboBox1 = comboBox1.SelectedIndex,
				BattleMapComboBox2 = comboBox2.SelectedIndex,
				BattleMapComboBox3 = comboBox3.SelectedIndex,
				BattleMapComboBox4 = comboBox4.SelectedIndex,
			};
			listBox1.Items.Add(battleMap.BattleMapComboBox1.ToString() + battleMap.BattleMapComboBox2.ToString() + battleMap.BattleMapComboBox3.ToString() + battleMap.BattleMapComboBox4.ToString());
		}

		public bool EnumChildWindowCallback(IntPtr hWnd, IntPtr lParam)
		{
			listBox2.Items.Add(hWnd);
			return true;
		}

		/// <summary>
		/// 护肝开始
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button6_Click(object sender, EventArgs e)
		{

		}

		/// <summary>
		/// 调试
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button9_Click(object sender, EventArgs e)
		{
			Wife_Core.Clicks(MainGameProgression, 170, 220);
		}

		private void button10_Click(object sender, EventArgs e)
		{

		}
	}
}
