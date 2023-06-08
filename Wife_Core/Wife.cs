using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;


namespace Wife_Core
{
	/// <summary>
	/// 鼠标控制
	/// </summary>
	public class Wife_mouse
	{
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

		/// <summary>
		/// 传入一个坐标，点击这个坐标下的位置
		/// </summary>
		/// <param name="pt"></param>
		public static void mouse_click(Point pt)
		{
			SetCursorPos(pt.X,pt.Y);
			mouse_event(MouseEventFlag.LeftDown | MouseEventFlag.LeftUp, 0, 0, 0, UIntPtr.Zero);
		}
	}

	/// <summary>
	/// 颜色控制
	/// </summary>
	public class Wife_color
	{
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
		static public System.Drawing.Color GetPixelColor(int x, int y)
		{
			IntPtr hdc = GetDC(IntPtr.Zero);
			uint pixel = GetPixel(hdc, x, y);
			ReleaseDC(IntPtr.Zero, hdc);
			Color color = Color.FromArgb((int)(pixel & 0x000000FF), (int)(pixel & 0x0000FF00) >> 8, (int)(pixel & 0x00FF0000) >> 16);
			return color;
		}
	}

	//主控制类
	public class Wife_Main
	{
		//公共变量
		public static string datapath = Directory.GetCurrentDirectory() + "\\Data";

		public static string Log = Directory.GetCurrentDirectory() + "\\Wife_Log.txt"; //日志文件路径

		/// <summary>
		/// 返回当前的页面情况 需要传入对应的界面色点分析文件夹路径
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static bool Home(string path)
		{
			//读取当前的文件夹下所有色点数据
			string[] file = Directory.GetFiles(path);
			//结果值，默认为真，如果一个不符合，则为假
			bool result = true;
			//遍历
			for (int i = 0; i < file.Length; i++)
			{
				//从游戏界面中获取的色值
				Point xy = P_info(file[i]);
				Color c = Wife_color.GetPixelColor(xy.X, xy.Y);
				//从数据内读出来的色值
				Color color = C_info(file[i]);
				//计算色差
				int cs = 10;
				int tmpr = Math.Abs(c.R - color.R);
				int tmpg = Math.Abs(c.G - color.G);
				int tmpb = Math.Abs(c.B - color.B);

				//色差RGB只要有一个大于色差，为假
				if ((tmpr > cs) || (tmpg > cs) || (tmpb > cs))
				{
					result = false;
					//调试专用
					//Console.WriteLine(file[i]);
					//MessageBox.Show(file[i]);
					//File.AppendAllText(Log, DateTime.Now + c.ToString() + file[i].ToString()+"\n");
					//File.AppendAllText(Log, DateTime.Now + color.ToString() + "\n");
				}
			}
			return result;
		}

		/// <summary>
		/// 传入一个文件路径，将这个路径内全部的信息分解为数组，返回坐标点位
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static Point P_info(string path)
		{
			string[] temp = File.ReadAllLines(path);
			Point point = new Point();
			point.X = int.Parse(temp[0]);
			point.Y = int.Parse(temp[1]);
			return point;
		}

		/// <summary>
		/// 返回颜色
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static Color C_info(string path)
		{
			string[] temp = File.ReadAllLines(path);
			Color c = Color.FromArgb(int.Parse(temp[2]), int.Parse(temp[3]), int.Parse(temp[4]));
			return c;
		}

		/// <summary>
		/// 屏幕截图
		/// </summary>
		/// <param name="info">1为放置在出船内，2为放置在修船内</param>
		public static void CreateImage(int info)
		{
			Image image = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
			Graphics g = Graphics.FromImage(image);
			g.CopyFromScreen(new Point(0, 0), new Point(0, 0), Screen.PrimaryScreen.Bounds.Size);
			string times = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ff");
			string path;
			if (info == 1)
			{
				//出船截图
				path = Directory.GetCurrentDirectory() + "\\Bmp_info\\Award\\";
				Directory.CreateDirectory(path);
			}
			else
			{
				//修船截图
				path = Directory.GetCurrentDirectory() + "\\Bmp_info\\Repair\\";
				Directory.CreateDirectory(path);
			}
			image.Save(path + times + ".jpg", ImageFormat.Jpeg);
		}
	}
}
