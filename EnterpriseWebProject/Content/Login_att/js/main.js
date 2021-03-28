$(function() {

    $('.btn-link[aria-expanded="true"]').closest('.accordion-item').addClass('active');
  $('.collapse').on('show.bs.collapse', function () {
	  $(this).closest('.accordion-item').addClass('active');
	});

  $('.collapse').on('hidden.bs.collapse', function () {
	  $(this).closest('.accordion-item').removeClass('active');
	});



});

$(document).on("submit","#frm-login", formValidate);
function formValidate(e){
  e.preventDefault();
    //validate the form
    $("#frm-login").validate(
        { submitHandler: function(form) {} }
);

$(document).on("submit","#frm-forgot", formValidate);
function formValidate(e){
  e.preventDefault();
    //validate the form
    $("#frm-forgot").validate(
        { submitHandler: function(form) {} }
);

$(document).on("submit","#frm-createpassword", formValidate);
function formValidate(e){
  e.preventDefault();
    //validate the form
    $("#frm-createpassword").validate(
        { submitHandler: function(form) {} }
);
