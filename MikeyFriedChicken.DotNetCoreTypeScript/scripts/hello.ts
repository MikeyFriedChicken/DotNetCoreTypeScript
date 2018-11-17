declare var module: any;
module.exports = function (callback: (error: any, result: any) => void, name: string) {
    var result: any = `Hello ${name}`;
    callback(null, `Hello ${name}`);
};

