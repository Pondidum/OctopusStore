namespace OctopusStore.Models
{
	public class ValueModel
	{
		public int CreateIndex { get; set; }
		public int ModifyIndex { get; set; }
		public int LockIndex { get; set; }
		public string Key { get; set; }
		public int Flags { get; set; }
		public string Value { get; set; }
		public string Session { get; set; }
	}
}
