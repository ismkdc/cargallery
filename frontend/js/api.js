$(document).on( "pagebeforeshow",(function() {
           $("#markalar").empty();
		$.get( "http://autodataservice.azurewebsites.net/auto/getbrands", function( data ) {
var $foo = $('#markalar');
        for(var i=0; i<data.length; i++) {
            $foo.append('<li><a href="#modeller" onclick="getModels(this)" id="'+data[i]['Id']+'">'+data[i]['Name']+'</a></li>');
        }
        $("#markalar").listview('refresh');
        $("#slider4").responsiveSlides({
        auto: false,
        pager: false,
        nav: true,
        speed: 500,
        namespace: "callbacks",
        
      });
}); 
        /* Slider */
        
}));

        function getModels(obj)
        {
            $("#model").empty();
         var brandId = $(obj).attr('id');
         $.get( "http://autodataservice.azurewebsites.net/auto/getmodels", {id:brandId}).done(function(data){
           
            var $foo = $('#model');
        for(var i=0; i<data.length; i++) {
            $foo.append('<li><a href="#modelinModelleri" onclick="getSubModels(this)" id="'+data[i]['Id']+'">'+data[i]['Name']+' <img src="'+ data[i]['Image']+'"></a></li>');
        }
        $("#model").listview('refresh');
});
        }
        function getSubModels(obj){
             $("#modelinModeli").empty();
         var modelId = $(obj).attr('id');
         $.get( "http://autodataservice.azurewebsites.net/auto/getsubmodels", {id:modelId}).done(function(data){
           

            var $foo = $('#modelinModeli');
        for(var i=0; i<data.length; i++) {
          if (data[i]['IsHeader']==true) {
           
            $foo.append('<li data-role="list-divider">'+data[i]['Name']+'</li>');
          }
          else {
            $foo.append('<li><a href="#detay" onclick="getCar(this)" id="'+data[i]['Id']+'">'+data[i]['Name']+'</a></li>');
          }
        }
        $("#modelinModeli").listview('refresh');
});
        }

        function getCar(obj){
         $("#carDetail").empty();
          $("#slider2").empty();
          $(".rslides_tabs").remove();
         var carId = $(obj).attr('id');
         $.get( "http://autodataservice.azurewebsites.net/auto/getcar", {id:carId}).done(function(data){
            var $foo = $('#carDetail');
            var $slide = $('#slider2');
            var InfoArray = data['Info'];
            var images = data['Images'];
			
			for(var key in InfoArray) 
			{
				$foo.append('<tr><td>'+key+'</td><td style="color:#a7a1ae;">'+InfoArray[key]+'</td></tr>');
			}

            for(var image in images){
                $slide.append('<li><a href="#"><img src="'+images[image]+'" alt=""></a></li>');
            }
		
            $("#slider2").responsiveSlides({
        auto: false,
        pager: true,
        speed: 300,
        maxwidth: 540
      });
        
});
        }