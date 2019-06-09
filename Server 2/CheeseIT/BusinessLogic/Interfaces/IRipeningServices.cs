using CheeseIT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseIT.BusinessLogic.Interfaces
{
    public interface IRipeningServices
    {
        Ripening CreateRipening(Guid cheeseId);

        Ripening FinishRipening(Guid ripeningId);

        Task<Ripening> GetCurrentRipeningModel();

        void ValidateMeasure(Measurement measure);
    }
}
