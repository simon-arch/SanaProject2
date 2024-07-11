export function getCurrentDate(format = 'default'){
    if (format == null) return null
    var newDate = (format === 'default') ? new Date() : new Date(format);
    var date = newDate.getDate();
    var month = newDate.getMonth() + 1;
    var year = newDate.getFullYear();

    var hours = newDate.getHours();
    var minutes = newDate.getMinutes();
    var seconds = newDate.getSeconds();
    
    var half = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12;
    
    minutes = minutes < 10 ? '0' + minutes : minutes;
    seconds = seconds < 10 ? '0' + seconds : seconds;

    return `${month}/${date}/${year} ${hours}:${minutes}:${seconds} ${half}`
}