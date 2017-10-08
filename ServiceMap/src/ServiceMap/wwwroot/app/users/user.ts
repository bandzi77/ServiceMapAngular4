export class IUser {
    public _id: string;
    public tntUserName: string = '';
    public email: string = '';
    public password: string = '';
    public isSuperUser = false;
    public isLocked = false;
    public limitOfRequestsPerDay?: number;
    public numberOfRequestsPerDay?: number;
}

export interface IUserFilter {
    email: string;
    showLockedOnly?: boolean;
}

export interface IUserResult {
    users: IUser[];
};