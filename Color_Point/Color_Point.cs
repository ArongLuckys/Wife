using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection.Emit;

namespace Color_Point
{
	public partial class Color_Point : Form
	{
		#region 外来接口

		//颜色相关
		[DllImport("user32.dll")]
		static extern IntPtr GetDC(IntPtr hwnd);
		[DllImport("user32.dll")]
		static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);
		[DllImport("gdi32.dll")]
		static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);


		/// <summary>
		/// 返回颜色色值
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		static public Color GetPixelColor(int x, int y)
		{
			IntPtr hdc = GetDC(IntPtr.Zero);
			uint pixel = GetPixel(hdc, x, y);
			ReleaseDC(IntPtr.Zero, hdc);
			Color color = Color.FromArgb((int)(pixel & 0x000000FF), (int)(pixel & 0x0000FF00) >> 8, (int)(pixel & 0x00FF0000) >> 16);
			return color;
		}

		//鼠标相关
		[DllImport("user32.dll")]
		static extern bool SetCursorPos(int X, int Y);
		[DllImport("user32.dll")]
		static extern void mouse_event(MouseEventFlag flags, int dx, int dy, uint data, UIntPtr extraInfo);
		[Flags]
		enum MouseEventFlag : uint
		{
			Move = 0x0001,
			LeftDown = 0x0002,
			LeftUp = 0x0004,
			RightDown = 0x0008,
			RightUp = 0x0010,
			MiddleDown = 0x0020,
			MiddleUp = 0x0040,
			XDown = 0x0080,
			XUp = 0x0100,
			Wheel = 0x0800,
			VirtualDesk = 0x4000,
			Absolute = 0x8000
		}

		[DllImport("user32.dll")]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("user32.dll")]
		public static extern int GetClassName(IntPtr hWnd, out string lpClassName, int nMaxCount);

		[DllImport("user32.dll")]
		public static extern bool EnumChildWindows(IntPtr hWndParent, EnumChildProc lpEnumFunc, IntPtr lParam);
		public delegate bool EnumChildProc(IntPtr hWnd, IntPtr lParam);

		[DllImport("user32.dll")]
		public static extern int GetWindowTextLength(IntPtr hwnd);

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

		[DllImport("user32.dll")]
		public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);




		public static bool EnumChildWindowCallback(IntPtr hWnd, IntPtr lParam)
		{
			if (!list_hW.Contains(hWnd.ToString()))
			{
				list_hW.Add(hWnd.ToString());
			}
			return true;
		}

		public struct RECT
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}

		#endregion

		public Color_Point()
		{
			InitializeComponent();
		}

		//公共变量
		public Color color, color2, color3, color4, color5, color6, color7, color8, color9;
		public string value;

		/// <summary>
		/// 存储当前鼠标位置的点位信息
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button1_Click(object sender, EventArgs e)
		{
			listBox1.Items.Add(value);
		}


		static List<string> list_hW = new List<string>();

		/// <summary>
		/// 传入主窗体的句柄，返回要推送的子窗口句柄
		/// </summary>
		/// <returns></returns>
		public static IntPtr GetSubform(IntPtr hWnd)
		{
			if (hWnd != IntPtr.Zero)
			{
				EnumChildWindows(hWnd, EnumChildWindowCallback, IntPtr.Zero);
				for (int i = 0; i < list_hW.Count; i++)
				{
					IntPtr ip = new IntPtr(int.Parse(list_hW[i]));
					int length = GetWindowTextLength(ip);
					StringBuilder title = new StringBuilder(length + 1);
					if (length > 0)
					{
						GetWindowText(ip, title, length + 1);
						if (title.ToString() == "TheRender")
						{
							return ip;
						}
					}
				}
			}
			return hWnd;
		}

		/// <summary>
		/// 主程序
		/// </summary>
		public void StartiInfo()
		{
			IntPtr ips = GetSubform(FindWindow(null, textBox3.Text));
			RECT rect = new RECT();
			GetWindowRect(ips, ref rect);
			label7.Text = ips.ToString();
			label8.Text = "宽度" + (rect.Right - rect.Left).ToString() + "高度" + (rect.Bottom - rect.Top).ToString();
			label9.Text = "上下坐标" + rect.Top + "左右坐标" + rect.Left;

			//获取光标位置
			Point mousePosition = new Point(Control.MousePosition.X - rect.Left, Control.MousePosition.Y - rect.Top);

			//主要的颜色显示
			color = GetPixelColor(Control.MousePosition.X, Control.MousePosition.Y);

			color2 = GetPixelColor(Control.MousePosition.X - 1, Control.MousePosition.Y - 1);
			color3 = GetPixelColor(Control.MousePosition.X, Control.MousePosition.Y - 1);
			color4 = GetPixelColor(Control.MousePosition.X + 1, Control.MousePosition.Y - 1);
			color5 = GetPixelColor(Control.MousePosition.X - 1, Control.MousePosition.Y);
			color6 = GetPixelColor(Control.MousePosition.X + 1, Control.MousePosition.Y);
			color7 = GetPixelColor(Control.MousePosition.X - 1, Control.MousePosition.Y + 1);
			color8 = GetPixelColor(Control.MousePosition.X, Control.MousePosition.Y + 1);
			color9 = GetPixelColor(Control.MousePosition.X + 1, Control.MousePosition.Y + 1);

			button9.BackColor = color;
			button5.BackColor = color2;
			button6.BackColor = color3;
			button7.BackColor = color4;
			button10.BackColor = color5;
			button8.BackColor = color6;
			button13.BackColor = color7;
			button12.BackColor = color8;
			button11.BackColor = color9;

			button14.BackColor = Color.Red;

			if ((color == color2) && (color == color3) && (color == color4) && (color == color5) && (color == color6) && (color == color7) && (color == color8) && (color == color9))
			{
				button14.BackColor = Color.Green;
			}

			//得到rgp
			int red = color.R;    //R值 
			int green = color.G; //G值 
			int blue = color.B;  //B值
			string cssvalue = System.Drawing.ColorTranslator.ToHtml(GetPixelColor(mousePosition.X, mousePosition.Y));  //将颜色转换为CSS字符串
			label2.Text = mousePosition.X.ToString() + "\\"
				+ mousePosition.Y.ToString() + "\\"
				+ red.ToString() + "\\"
				+ green.ToString() + "\\"
				+ blue.ToString();
			value = mousePosition.X.ToString() + "=" + mousePosition.Y.ToString() + "=" + color.R.ToString() + "=" + color.G.ToString() + "=" + color.B.ToString();

			GC.Collect();
		}

		/// <summary>
		/// 保存当前鼠标位置的点位信息到文件夹下面
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button2_Click(object sender, EventArgs e)
		{
			//文件类型
			string path = Directory.GetCurrentDirectory() + "\\" + "Wife_Color_Point_Data" + ".wife";

			if (listBox1.Items.Count != 0)
			{
				if (File.Exists(path))
				{
					File.Delete(path);
				}

				for (int i = 0; i < listBox1.Items.Count; i++)
				{
					File.AppendAllText(path, listBox1.Items[i].ToString() + "\n");
				}
				MessageBox.Show("创建完成");
			}
		}

		/// <summary>
		/// 主分析计时器
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timer1_Tick(object sender, EventArgs e)
		{
			StartiInfo();
		}

		/// <summary>
		/// 打开文件地址
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button3_Click(object sender, EventArgs e)
		{
			string files = Directory.GetCurrentDirectory();
			System.Diagnostics.Process.Start("explorer.exe", files);
		}

		/// <summary>
		/// 检测，根据输入的xy获取色号
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button4_Click(object sender, EventArgs e)
		{
			int x = int.Parse(textBox1.Text);
			int y = int.Parse(textBox2.Text);
			Point pt = new Point(x, y);
			color = GetPixelColor(pt.X, pt.Y);
			int red = color.R;    //R值 
			int green = color.G; //G值 
			int blue = color.B;  //B值
			label5.Text = red + "\\" + green + "\\" + blue;
		}
	}
}
