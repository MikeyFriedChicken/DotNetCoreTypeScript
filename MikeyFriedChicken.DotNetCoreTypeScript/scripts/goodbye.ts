declare var module: any;
var goodbye: any = (callback: (error: any, result: any) => void, name: string) => {
    var result: any = `Goodbye ${name}`;
    callback(null, `Goodbye ${name}`);
};
export = goodbye;


