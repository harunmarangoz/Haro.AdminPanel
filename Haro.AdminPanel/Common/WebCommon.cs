using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using Haro.AdminPanel.Business.Extensions;
using Haro.AdminPanel.Business.Managers;
using Haro.AdminPanel.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Haro.AdminPanel.Common
{
    public class WebCommon
    {
        private IHttpContextAccessor _httpContextAccessor;
        private UserManager _userManager;
        private SiteInformationManager _siteInformationManager;
        private readonly LanguageManager _languageManager;

        public WebCommon(IHttpContextAccessor httpContextAccessor, UserManager userManager,
            SiteInformationManager siteInformationManager, LanguageManager languageManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _siteInformationManager = siteInformationManager;
            _languageManager = languageManager;
        }

        private User _user;

        public User User
        {
            get
            {
                if (_user == null)
                {
                    var userId = Convert.ToInt64(_httpContextAccessor.HttpContext.User.Claims
                        .Where(c => c.Type == ClaimTypes.NameIdentifier)
                        .Select(c => c.Value).SingleOrDefault());
                    var user = _userManager.BaseQuery().Include(x => x.Modules).GetById(userId);
                    _user = user;
                }

                return _user;
            }
        }

        private SiteInformation _siteInformation;

        public SiteInformation SiteInformation
        {
            get
            {
                if (_siteInformation == null)
                {
                    _siteInformation = _siteInformationManager.Get(x => true);
                }

                return _siteInformation;
            }
        }

        private List<Language> _languages;

        public Language Language
        {
            get
            {
                if (_languages == null)
                {
                    _languages = _languageManager.List();
                }
                return _languages.First(x => x.Code == CultureInfo.CurrentUICulture.Name);
            }
        }

        public List<Language> Languages
        {
            get
            {
                if (_languages == null)
                {
                    _languages = _languageManager.List();
                }

                return _languages;
            }
        }

        public void Clear()
        {
            _user = null;
            _siteInformation = null;
            _languages = null;
        }
    }
}