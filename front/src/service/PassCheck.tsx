const format = /[ `!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?~]/; 
const numbers = /[0-9]/;

function passCheck(arg: string){
    var containsUpperLetters = false;
    var containsLowerLetters = false;
    var containsNumbers = false;
    var containsSpecialChars = false;
    var isAtLeastEightLetter = false;
    var isBadPass = false;

    for (let i = 0; i < arg.length; i++) {
        if(arg[i] == arg[i].toLocaleLowerCase()) containsLowerLetters = true;
        if(arg[i] == arg[i].toLocaleUpperCase()) containsUpperLetters = true;
    }
    containsNumbers = numbers.test(arg);
    containsSpecialChars = format.test(arg);
    isAtLeastEightLetter = arg.length >= 8;     
    return {'containsUpperLetters': containsUpperLetters,
            'containsLowerLetters': containsLowerLetters,
            'containsNumbers'     : containsNumbers,
            'containsSpecialChars': containsSpecialChars,
            'isAtLeastEightLetter': isAtLeastEightLetter,
            'isGoodPass'           : !isBadPass}   
}

function passStrenght(arg: string){
    var UpperLetters = 0;
    var LowerLetters = 0;
    var Numbers = 0;
    var SpecialChars = 0;
    var charSet = new Set<string>();

    for (let i = 0; i < arg.length; i++) {
        if(arg[i] == arg[i].toLocaleLowerCase()) UpperLetters++;
        if(arg[i] == arg[i].toLocaleUpperCase()) LowerLetters++;
        if(numbers.test(arg[i])) Numbers++;
        if(format.test(arg[i])) SpecialChars++;
        charSet.add(arg[i]);
    }

    var strenght = 1;
    strenght *= UpperLetters;
    strenght *= LowerLetters;
    strenght *= Numbers;
    strenght *= SpecialChars;
    strenght *= charSet.size;
    return 1 - 1/strenght;
}

export {passCheck, passStrenght};