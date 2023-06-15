using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Wife_Main
{
	public class Wife_Core
	{
		[DllImport("user32.dll")]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("user32.dll")]
		public static extern int GetClassName(IntPtr hWnd, out string lpClassName, int nMaxCount);

		[DllImport("user32.dll")]
		public static extern int GetWindowTextLength(IntPtr hwnd);

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

		[DllImport("user32.dll")]
		public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

		[DllImport("user32.dll")]
		public static extern IntPtr GetDC(IntPtr hWnd);

		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

		[DllImport("gdi32.dll")]
		public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

		[DllImport("gdi32.dll")]
		public static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);

		[DllImport("user32.dll")]
		public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

		[DllImport("gdi32.dll")]
		public static extern int DeleteDC(IntPtr hdc);

		[DllImport("gdi32.dll")]
		public static extern int DeleteObject(IntPtr hObject);

		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		[DllImport("user32.dll")]
		static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

		[DllImport("user32.dll")]
		static extern bool SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);

		[DllImport("gdi32.dll")]
		static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

		public struct RECT
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		public static extern bool EnumChildWindows(IntPtr hWndParent, EnumChildProc lpEnumFunc, IntPtr lParam);
		public delegate bool EnumChildProc(IntPtr hWnd, IntPtr lParam);

		/// <summary>
		/// 传入线程int，返回这个线程的image类型
		/// </summary>
		/// <param name="hWnd"></param>
		/// <returns></returns>
		public static Image CaptureWindow(IntPtr hWnd)
		{
			RECT rect;
			GetWindowRect(hWnd, out rect);
			int width = rect.Right - rect.Left;
			int height = rect.Bottom - rect.Top;

			IntPtr hdcSrc = GetDC(hWnd);
			IntPtr hdcDest = CreateCompatibleDC(hdcSrc);
			IntPtr hBitmap = CreateCompatibleBitmap(hdcSrc, width, height);
			IntPtr hOld = SelectObject(hdcDest, hBitmap);
			BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, 0x00CC0020);
			SelectObject(hdcDest, hOld);
			Image image = Image.FromHbitmap(hBitmap);
			DeleteObject(hBitmap);
			DeleteDC(hdcDest);
			ReleaseDC(hWnd, hdcSrc);
			return image;
		}

		private const uint WM_LBUTTONDOWN = 0x201;
		private const uint WM_LBUTTONUP = 0x202;
		private const uint MK_LBUTTON = 0x0001;

		/// <summary>
		/// 向一个线程发送含有位置度的坐标点击消息
		/// </summary>
		/// <param name="ass"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public static void Clicks(IntPtr ass, Point p)
		{
			IntPtr lParam = (IntPtr)((p.Y << 16) | p.X); // 将x和y坐标合并为lParam参数
			IntPtr wParam = (IntPtr)MK_LBUTTON; // 设置鼠标左键按下标志位
			SendMessage(ass, WM_LBUTTONDOWN, wParam, lParam);
			SendMessage(ass, WM_LBUTTONUP, wParam, lParam);
		}

		static List<string> list_hW = new List<string>();

		/// <summary>
		/// 传入主窗体的句柄，返回要推送的子窗口句柄,这个目前只支持雷电模拟器！
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
					int length = Wife_Core.GetWindowTextLength(ip);
					StringBuilder title = new StringBuilder(length + 1);
					if (length > 0)
					{
						Wife_Core.GetWindowText(ip, title, length + 1);
						if (title.ToString() == "TheRender")
						{
							return ip;
						}
					}
				}
			}
			return hWnd;
		}

		public static bool EnumChildWindowCallback(IntPtr hWnd, IntPtr lParam)
		{
			if (!list_hW.Contains(hWnd.ToString()))
			{
				list_hW.Add(hWnd.ToString());
			}
			return true;
		}

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
			SetCursorPos(pt.X, pt.Y);
			mouse_event(MouseEventFlag.LeftDown | MouseEventFlag.LeftUp, 0, 0, 0, UIntPtr.Zero);
		}

		/// <summary>
		/// 返回颜色色值
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		static public Color GetPixelColor(Point p)
		{
			//IntPtr hdc = GetDC(IntPtr.Zero);
			uint pixel = GetPixel(Main.MainGameProgression, p.X, p.Y);
			ReleaseDC(IntPtr.Zero, Main.MainGameProgression);
			Color color = Color.FromArgb((int)(pixel & 0x000000FF), (int)(pixel & 0x0000FF00) >> 8, (int)(pixel & 0x00FF0000) >> 16);
			return color;
		}

		//公共变量
		public static string datapath = Directory.GetCurrentDirectory() + "\\Data";
		public static string Log = Directory.GetCurrentDirectory() + "\\Wife_Log.txt"; //日志文件路径

		/// <summary>
		/// 返回当前的页面情况 需要传入对应的界面色点分析文件路径
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static bool Home(string path)
		{
			//读取当前的文件下所有色点数据
			string[] file = File.ReadAllLines(path);
			//结果值，默认为真，如果一个不符合，则为假
			bool result = true;
			for (int i = 0; i < file.Length; i++)
			{
				string[] strArr = file[i].Split('=');
				int[] intArr = new int[5];
				for (int j = 0; j < strArr.Length; j++)
				{
					intArr[j] = int.Parse(strArr[j]);
				}

				//从游戏界面中获取的色值
				Point xy =new Point(intArr[0], intArr[1]);
				Color c = new Color();
				if (Main.LiveInterface.Size != new Size(1, 1))
				{
					c = ((Bitmap)Main.LiveInterface).GetPixel(xy.X, xy.Y);
				}

				//从数据内读出来的色值
				Color color = Color.FromArgb(intArr[2], intArr[3], intArr[4]);
				//计算色差
				int cs = 10;
				int tmpr = Math.Abs(c.R - color.R);
				int tmpg = Math.Abs(c.G - color.G);
				int tmpb = Math.Abs(c.B - color.B);

				//色差RGB只要有一个大于色差，为假
				if ((tmpr > cs) || (tmpg > cs) || (tmpb > cs))
				{
					result = false;
					break;
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
			Point point = new Point(int.Parse(temp[0]), int.Parse(temp[1]));
			return point;
		}

		/// <summary>
		/// 还原数据中的RGB并且组合成Color
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
		/// <param name="info">1为放置在出船内，2为放置在修船内,3为远征，0为其他</param>
		public static void CreateImage(int info)
		{
			string times = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ff");
			string path;
			switch (info)
			{
				case 1:
					{
						path = Directory.GetCurrentDirectory() + "\\Bmp_info\\Award\\";
						Directory.CreateDirectory(path);
					}; break;
				case 2:
					{
						path = Directory.GetCurrentDirectory() + "\\Bmp_info\\Repair\\";
						Directory.CreateDirectory(path);
					}; break;
				case 3:
					{
						path = Directory.GetCurrentDirectory() + "\\Bmp_info\\Expedition\\";
						Directory.CreateDirectory(path);
					}; break;
				default:
					{
						path = Directory.GetCurrentDirectory() + "\\Bmp_info\\Else\\";
						Directory.CreateDirectory(path);
					}; break;
			}
			Main.LiveInterface.Save(path + times + ".jpg", ImageFormat.Jpeg);
		}
	}
}
