declare var module: any;
module.exports = function (callback: (error: any, result: any) => void, name: string) {
    var result = `Hello ${name}`;
    //var error: any = null;
    callback(null, `Hello ${name}`);
}

