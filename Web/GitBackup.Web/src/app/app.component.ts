import {Component, OnInit} from '@angular/core';
import {ToastrService} from 'ngx-toastr';
import {BackupsService} from './backups.service';
import {RepositoriesService} from './repositories.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  public repositories: IRepository[] = [];

  constructor(
    private repositoriesService: RepositoriesService,
    private backupsService: BackupsService,
    private toastrService: ToastrService) {
  }

  public ngOnInit(): void {
    this.fetchRepositories();
  }

  public backup(repositoryId: number): void {
    this.backupsService.create(repositoryId)
      .subscribe({
        next: () => {
          this.toastrService.success('Backup created');
          this.fetchRepositories();
        },
        error: () => {
          this.toastrService.error('Backup creation failed');
        }
      });
  }

  public restore(repositoryId: number): void {
    this.backupsService.restore(repositoryId)
      .subscribe({
        next: () => {
          this.toastrService.success('Backup restored');
          this.fetchRepositories();
        },
        error: () => {
          this.toastrService.error('Backup restore failed');
        }
      });
  }

  private fetchRepositories(): void {
    this.repositoriesService.getRepositories()
      .subscribe(repositories => {
        this.repositories = repositories;
      });
  }
}
