using System;
using System.Net;
using Foundation;
using ObjCRuntime;
using CookieWebView.iOS;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(WebView), typeof(CustomWebViewRenderer))]
namespace CookieWebView.iOS
{
    public class CustomWebViewRenderer : WkWebViewRenderer
    {
        public CustomWebViewRenderer()
        {
            SetCookies();
        }
        
        // 웹뷰 내에서 화면 넘어갈 때마다 델리게이트 발생
        // (Deligate occurred.)
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            NavigationDelegate = new CustomNavigationDelegate();
        }

        // 앱 켜질 때 받아오는 쿠키 삭제하고 기존 쿠키 삽입하는 메서드
        // (Method to delete incoming cookies and insert existing cookies when the app is turned on)
        private static async void SetCookies()
        {
            var dataStore = WKWebsiteDataStore.DefaultDataStore;
            var cookies = NSHttpCookieStorage.SharedStorage.Cookies;
            var oldCookies = await dataStore.HttpCookieStore.GetAllCookiesAsync();
            
            foreach (var cookie in oldCookies)
            {
                await dataStore.HttpCookieStore.DeleteCookieAsync(cookie);
            }
            
            foreach (var cookie in cookies)
            {
                await dataStore.HttpCookieStore.SetCookieAsync(cookie);
            }
        }
    }
    
    // 네비게이션 델리게이트 커스텀 (Custom Navigation Delegate)
    // 화면 넘어갈 때 해당 페이지 쿠키 불러와서 다시 저장. (Save cookies if they are on the page when the screen goes over.)
    public class CustomNavigationDelegate : WKNavigationDelegate
    {
        public override void DecidePolicy(WKWebView webView, WKNavigationResponse navigationResponse,
            [BlockProxy(typeof(Action))] Action<WKNavigationResponsePolicy> decisionHandler)
        {
            var wKHttpCookieStore = webView.Configuration.WebsiteDataStore.HttpCookieStore;

            wKHttpCookieStore.GetAllCookies(cookies =>
            {
                if (cookies.Length <= 0) return;
                foreach (var cookie in cookies)
                {
                    // iOS에서는 쿠키에 만료 시간 없으면 저장 X (In iOS, cookies are not saved if they are free to expire.)
                    // 쿠키에 만료 기간 없을 경우 임의로 만료기간 세팅 (Randomly set expiration dates if cookies do not have expiration dates.)
                    var editCookie = new NSHttpCookie (new Cookie {
                        Version = (int) cookie.Version,
                        Name = cookie.Name,
                        Value = cookie.Value,
                        Expires = DateTime.Now + TimeSpan.FromDays(30),
                        Domain = cookie.Domain,
                        Path = cookie.Path,
                        Secure = cookie.IsSecure
                    });
                    
                    NSHttpCookieStorage.SharedStorage.SetCookie(editCookie);
                }
            });
            decisionHandler(WKNavigationResponsePolicy.Allow);
        }
    }
}