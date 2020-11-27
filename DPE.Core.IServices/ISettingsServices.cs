using DPE.Core.IServices.BASE;
using DPE.Core.Model.Models;
using System.Threading.Tasks;

namespace DPE.Core.IServices
{	
	/// <summary>
	/// ISettingsServices
	/// </summary>	
    public interface ISettingsServices :IBaseServices<Settings>
	{
		Task<Settings> GetSettings();
    }
}