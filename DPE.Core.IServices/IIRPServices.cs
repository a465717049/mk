using DPE.Core.IServices.BASE;
using DPE.Core.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace DPE.Core.IServices
{	
	/// <summary>
	/// IIRPServices
	/// </summary>	
    public interface IIRPServices  : IBaseServices<IRP>
	{
		Task<bool> SetAdSignIn(long uid);
	}
}