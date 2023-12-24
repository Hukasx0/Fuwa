import { PostedBy } from "./postedBy"

export class Post {
    id: number = 0
    postedBy: PostedBy = new PostedBy
    title: string = ""
    text: string = ""
    createdDate: Date = new Date()
    lastModifiedDate: Date = new Date()
}