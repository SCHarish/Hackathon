$(function () {
    
    $('#street').keyboard({
        layout: 'custom',
        customLayout: {
            
            'normal': [
                '1 2 3 4 5 6 7 8 9 0 - {bksp}',
                ' Q W E R T Y U I O P ',
                'A S D F G H J K L ',
                ' Z X C V B N M . ',
                '{accept} {space} {cancel} /'
            ]
        },
        display:
        {
        	'bksp':'Backspace',
			'accept':'OK'
        },
     
        autoAccept:true,

      
        repeatRate: 0
    });
	
    $('#email').keyboard({
        layout: 'custom',
        customLayout: {
            'normal': [
               ' 1 2 3 4 5 6 7 8 9 0 - {bksp}',
               '@ q w e r t y u i o p ',
               'a s d f g h j k l ',
               ' _ z x c v b n m . ',
               '{accept} {space} {cancel}'
            ]
           
        },
         display:
        {
        	'bksp':'Backspace',
			'accept':'OK'
        },
		autoAccept:true,
        repeatRate: 0
    });

    $('#name').keyboard({
        layout: 'custom',
        customLayout: {
            
            'normal': [
                ' {bksp}',
                ' Q W E R T Y U I O P ',
                'A S D F G H J K L ',
                ' Z X C V B N M . ',
                '{accept} {space} {cancel} '
            ]
        },
         display:
        {
        	'bksp':'Backspace',
			'accept':'OK'
        },
		autoAccept:true,
        repeatRate: 0
    });

    $('#nextofkin').keyboard({
        layout: 'custom',
        customLayout: {
           
            'normal': [
                ' {bksp}',
                ' Q W E R T Y U I O P ',
                'A S D F G H J K L',
                'Z X C V B N M .',
                '{accept} {space} {cancel} '
            ]
        },
         display:
        {
        	'bksp':'Backspace',
			'accept':'OK'
        },
		autoAccept:true,
        repeatRate: 0
    });
   
    $('#city').keyboard({
        layout: 'custom',
        customLayout: {
           
            'normal': [
                ' {bksp}',
                ' Q W E R T Y U I O P ',
                'A S D F G H J K L',
                ' Z X C V B N M . ',
                '{accept} {space} {cancel} '
            ]
        },
         display:
        {
        	'bksp':'Backspace',
			'accept':'OK'
        },
		autoAccept:true,
        repeatRate: 0
    });
    $('#locality').keyboard({
        layout: 'custom',
        customLayout: {
          
            'normal': [
                ' {bksp}',
                ' Q W E R T Y U I O P ',
                'A S D F G H J K L',
                ' Z X C V B N M .',
                '{accept} {space} {cancel} '
            ]
        },
         display:
        {
        	'bksp':'Backspace',
			'accept':'OK'
        },
		autoAccept:true,
        repeatRate: 0
    });
    $('#pin').keyboard({
        layout: 'custom',
        customLayout: {
            'normal': [

                '1 2 3',
                '4 5 6',
                '7 8 9',
                '0 ',
                '{bksp} {accept} {cancel}',

            ]
        },
         display:
        {
        	'bksp':'Backspace',
			'accept':'OK'
        },
		autoAccept:true,
        repeatRate: 0
    });
    $('#landline').keyboard({
        layout: 'custom',
        customLayout: {
            'normal': [

                '1 2 3',
                '4 5 6',
                '7 8 9',
                '0 + -',
                '{bksp} {accept} {cancel}',

            ]
        },
         display:
        {
        	'bksp':'Backspace',
			'accept':'OK'
        },
		autoAccept:true,
        repeatRate: 0
    });
    $('#mobile').keyboard({
        layout: 'custom',
        customLayout: {
            'normal': [

                '1 2 3',
                '4 5 6',
                '7 8 9',
                '0 + -',
                '{bksp} {accept} {cancel}',

            ]
        },
         display:
        {
        	'bksp':'Backspace',
			'accept':'OK'
        },
		autoAccept:true,
        repeatRate: 0
    });
    //  $('#age').keydown(false);
    

});