export function capitalizeFirstLetter(string) {
    let substrings = string.split(' ');
    if (substrings.length > 1) {
        console.log(substrings);
        for (let index = 0; index < substrings.length; index++) {
            const subString = substrings[index];
            substrings[index] = subString.charAt(0).toUpperCase() + subString.slice(1) + ' ';
        }
        return substrings.join('');
    }
    return string.charAt(0).toUpperCase() + string.slice(1);
}