using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.Services;

namespace TDEduEnglish.ViewModels
{
    class CommunityViewModel  {
        private readonly AppNavigationService _appNavigationService;
        public CommunityViewModel(AppNavigationService appNavigationService) {
            _appNavigationService = appNavigationService;
        }
    }
}
