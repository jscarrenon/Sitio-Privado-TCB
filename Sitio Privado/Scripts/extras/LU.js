$(document).ready(function() {
	//jQuery objects
	var $desktopButtonOpen 	= $('#LU-desktop-button');
	var $desktopDropdown 	= $('#LU-desktop-dropdown');
	var $desktopButtonClose	= $desktopDropdown.find('.LU-dropdown-header div');
	var $mobileButtonOpen 	= $('#LU-mobile-button');
	var $mobileDropdown 	= $('#LU-mobile-dropdown');
	var $mobileButtonClose 	= $mobileDropdown.find('.LU-dropdown-header div');
	//initial state
	var initialState = 0;
	//desktop
	$desktopButtonOpen.click(function(event) {
		if (initialState==0) {
			$desktopDropdown.slideDown();
			$mobileDropdown.css('display', 'block');
			initialState = 1;	
		} else {
			$desktopDropdown.slideUp();
			$mobileDropdown.css('display', 'none');
			initialState = 0;
		}
	});
	$desktopButtonClose.click(function(event) {
		$desktopDropdown.slideUp();
		$mobileDropdown.css('display', 'none');
		initialState = 0;
	});
	//mobile
	$mobileButtonOpen.click(function(event) {
		if (initialState==0) {
			$mobileDropdown.slideDown();
			$desktopDropdown.css('display', 'block');
			initialState = 1;	
		} else {
			$mobileDropdown.slideUp();
			$desktopDropdown.css('display', 'none');
			initialState = 0;
		}
	});
	$mobileButtonClose.click(function(event) {
		$mobileDropdown.slideUp();
		$desktopDropdown.css('display', 'none');
		initialState = 0;
	});
});