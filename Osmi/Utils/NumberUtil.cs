namespace Osmi.Utils;

[PublicAPI]
public static class NumberUtil {
	public static bool InRange(this int self, int min, int max) =>
		self >= min && self <= max;
	public static bool InRange(this short self, short min, short max) =>
		self >= min && self <= max;
	public static bool InRange(this long self, long min, long max) =>
		self >= min && self <= max;
	public static bool InRange(this float self, float min, float max) =>
		self >= min && self <= max;
	public static bool InRange(this double self, double min, double max) =>
		self >= min && self <= max;

	public static bool InRangeExclusive(this int self, int min, int max) =>
		self > min && self < max;
	public static bool InRangeExclusive(this short self, short min, short max) =>
		self > min && self < max;
	public static bool InRangeExclusive(this long self, long min, long max) =>
		self > min && self < max;
	public static bool InRangeExclusive(this float self, float min, float max) =>
		self > min && self < max;
	public static bool InRangeExclusive(this double self, double min, double max) =>
		self > min && self < max;

	public static int Clamp(this int self, int min, int max) =>
		self < min ? min : self > max ? max : self;
	public static short Clamp(this short self, short min, short max) =>
		self < min ? min : self > max ? max : self;
	public static long Clamp(this long self, long min, long max) =>
		self < min ? min : self > max ? max : self;
	public static float Clamp(this float self, float min, float max) =>
		self < min ? min : self > max ? max : self;
	public static double Clamp(this double self, double min, double max) =>
		self < min ? min : self > max ? max : self;
}
