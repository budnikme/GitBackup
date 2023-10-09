import {Component, OnInit} from '@angular/core';
import {GitHubService} from "./git-hub.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  public repositories: IRepository[] = [];

  constructor(private github: GitHubService) { }

  public ngOnInit(): void {
    this.github.getRepositories()
      .subscribe(repositories => {
        this.repositories = repositories;
    });
  }

  public backup(id: number): void {
    console.log(id);
  }

  public restore(id: number): void {
  }


}
