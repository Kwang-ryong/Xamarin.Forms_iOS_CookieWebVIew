# Xamarin.Forms iOS에서 웹쿠키 사용하기
# Xamarin.Forms iOS use web cookie

> 프로젝트 테스트 환경 (Project Environment)
- Mac OS Bigsur
- Rider 2021.1.2
- .NET 5
- Xamarin.Forms 5.0.0

> 설명    


자마린에서 사용되는 기본 웹뷰에서 쿠키를 저장하고 불러오는 커스텀 렌더러입니다. 여러 방법 중 가장 잘 작동하던 방법을 업로드합니다.  
_A custom renderer that stores and retrieves cookies from the default iOS web view used in Xamarin._  


SetCookies 메서드에서 처음 앱이 시작할 때 처음 받아오는 쿠키를 삭제하고 기존 쿠키를 셋팅합니다.  
_Method to delete incoming cookies and insert existing cookies when the app is turned on_  


네비게이션 델리게이트를 커스텀하여 화면이 넘어갈 때 새 쿠키를 받아옵니다.  
_Customize the navigation relay to receive new cookies as they cross the screen._  


>참고링크
- [stack overflow Christine's Answer](https://stackoverflow.com/questions/26573137/can-i-set-the-cookies-to-be-used-by-a-wkwebview)
- [stack overflow Junior Jiang's Answer](https://stackoverflow.com/questions/59741719/how-to-copy-cookie-to-wkwebview-in-ios)