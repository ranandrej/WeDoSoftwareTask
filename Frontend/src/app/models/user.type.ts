//Tipovi potrebni za registraciju i update usera
export type User = {
  id: string;
  name: string;
  surename: string;
  email: string;
};

export type UpdateUserDto = {
  name?: string;
  surename?: string;
  email?: string;
  password?: string;
  confirmPassword?: string;
};
