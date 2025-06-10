export interface RoomDTO {
  id: number;
  capacity: number;
  quantity: number;
}
export interface DeskDTO {
  id: number;
  quantity: number;
}
export interface WorkspaceResponse {
  id: number;
  name: string;
  description: string;
  workSpaceType: number;
  amenities: string[];
  photoList: string[];
  descCount: number;
  availabilityRooms: RoomDTO[];
  availabilityDesks: DeskDTO[];
  capacity: number[];
}
