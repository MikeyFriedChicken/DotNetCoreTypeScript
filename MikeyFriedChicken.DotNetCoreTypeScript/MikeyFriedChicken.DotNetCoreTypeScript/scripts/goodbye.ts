declare var module: any;
var goodbye = (callback: (error: any, result: any) => void, name: string) => {
    var result = `Goodbye ${name}`;
    //var error: any = null;
    callback(null, `Goodbye ${name}`);
}
export = goodbye;


